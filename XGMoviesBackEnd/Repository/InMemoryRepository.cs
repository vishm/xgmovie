using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XGMoviesBackEnd.ExternalServices;
using XGMoviesBackEnd.Domain;

namespace XGMoviesBackEnd.Repository
{
    public class InMemoryRepository : IMovieRepository
    {
        IMovieIDResolutionService _idResolution;
        int _movieUniqueCount = 0;
        List<Movie> _movies = new List<Movie>();

        public InMemoryRepository(IMovieIDResolutionService idResolutionService, bool seed = true)
        {
            if (seed)
            {
                _movies.AddRange(new List<Movie>()
                {
                    new Movie() { Id = ++_movieUniqueCount, Title = "Jurassic World",  Year = 2015},
                    new Movie() { Id = ++_movieUniqueCount, Title = "Deadpool",  Year = 2016},
                    new Movie() { Id = ++_movieUniqueCount, Title = "Iron Man",  Year = 2008},
                });
            }

            _idResolution = idResolutionService;
        }

        public List<Movie> GetAllMovies()
        {
            return _movies;
        }

        public Movie GetMovie(int Id)
        {
            // prefer SingleOrDefault as each Id should be unique
            return _movies.Where(movie => movie.Id == Id).SingleOrDefault();
        }

        //
        public int Store(Movie movie)
        {
            if ( movie.Id == 0 )
            {

                var externalMovieId = GetExternalMovieId(_idResolution, movie.Title, movie.Year);
                movie.Id = ++_movieUniqueCount;
                movie.MovieDbId = externalMovieId;

                _movies.Add(movie);
            }

            return movie.Id;
        }

        private static int GetExternalMovieId(IMovieIDResolutionService ideService, String title, ushort year)
        {
            int id = -1;
            try
            {
                var task = ideService.GetMovieIdAsync(title, year);
                task.Wait();
                id = task.Result;
            }
            catch(AggregateException e)
            {
                // swallow exception as we're default to -1
                if (e.InnerExceptions.First() is ArgumentException)
                {
                    throw e.InnerExceptions.First() as ArgumentException;
                }
                                
                System.Diagnostics.Debug.Assert(false, "unexpected exception");
            }

            return id;
        }
    }
}
