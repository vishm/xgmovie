using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XGMoviesBackEnd.Repository;
using XGMoviesBackEnd.Models;
using XGMoviesBackEnd.ExternalServices;
using System.Diagnostics;

namespace XGMovies.Controllers
{
    public class MoviesController : ApiController
    {
        static IMovieRepository _repostiory;

        static MoviesController()
        {
            // TODO - Use dependency inject and singleton in future
            _repostiory = new InMemoryRepository(new TheMovieDbOrgService(), seed:false);
        }

        // GET api/movies
        public IEnumerable<Movie> Get() => _repostiory.GetAllMovies();
        
        // GET api/movies/5        
        public IHttpActionResult Get(int id)
        {
            IHttpActionResult retValue = NotFound();

            var value = _repostiory.GetMovie(id);            
            if ( value != null)
            {                
                retValue= Ok(value);
            }

            return retValue;
        }

        // POST api/movies
        public IHttpActionResult Post([FromBody]Movie movie)
        {
            const int YEAR_OF_FIRST_MOVIE = 1896;

            if (movie == null || String.IsNullOrWhiteSpace(movie.Title) || movie.Year < YEAR_OF_FIRST_MOVIE)
            {
                Trace.WriteLine(String.Format("Movie object failed validity checks {0}", (movie == null) ? "deserializtion failure" : ""), "POST");
                return BadRequest();
            }

            try
            {
                var id = _repostiory.Store(movie);
                return Ok(id);
            }
            catch(Exception e)
            {
                Trace.WriteLine($"Unable to Store object, exception: {e.Message}", "POST");
                return BadRequest();
            }
        }
    }
}
