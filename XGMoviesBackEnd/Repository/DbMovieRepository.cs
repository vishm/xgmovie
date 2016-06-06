using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XGMoviesBackEnd.DbDataAccess;
using XGMoviesBackEnd.Domain;
using System.Data.Entity.Migrations;
using XGMoviesBackEnd.ExternalServices;

namespace XGMoviesBackEnd.Repository
{
    public class DbMovieRepository : IMovieRepository
    {        
        IMovieIDResolutionService _idResolutionService;

        public DbMovieRepository(IMovieIDResolutionService idResolutionService)
        {            
            _idResolutionService = idResolutionService;
        }

        public List<Movie> GetAllMovies()
        {
            // As we're simply returning the data, and not
            // going to manipulate it in teh Context, no need
            // to track it.
            List<Movie> retList = null;
            using (var context = new MoviesContext())
            {
                retList = context.Movies.AsNoTracking().ToList();
            }

            return retList;
        }

        public Movie GetMovie(int movieDbId)
        {
            Movie retValue;
            using (var context = new MoviesContext())
            {
                retValue = context.Movies
                            .AsNoTracking()
                            .Where(x => x.Id == movieDbId)
                            .SingleOrDefault();
            }

            return retValue;
        }

        public int Store(Movie movie)
        {
            using (var context = new MoviesContext())
            {
                if (movie.Id == 0)
                {
                    var externalMovieId = GetExternalMovieId(_idResolutionService, movie.Title, movie.Year);
                    movie.TheMovideDbOrgId = externalMovieId;

                    // Bit rubbish, need to use a guid is better;
                    var nextId = context.Movies.Max(x => x.Id) + 1;
                    movie.Id = nextId;
                }

                context.Movies.AddOrUpdate(movie);
                context.SaveChanges();
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
            catch (AggregateException e)
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
