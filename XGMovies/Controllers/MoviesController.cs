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
    [RoutePrefix("api/movies")]
    public class MoviesController : ApiController
    {
        static IMovieRepository _repostiory;

        static MoviesController()
        {
            // TODO - Use dependency inject and singleton in future
            _repostiory = new InMemoryRepository(new TheMovieDbOrgService(), seed:false);
        }

        // GET api/movies
        [HttpGet]
        public IHttpActionResult Get() => Ok(_repostiory.GetAllMovies().AsEnumerable());

        // GET api/movies/5        
        [HttpGet, Route("{id:int}", Name ="GetById")]
        public IHttpActionResult Get(int id)
        {
            IHttpActionResult retValue = null;

            var value = _repostiory.GetMovie(id);            
            if ( value != null)
            {
                retValue = Ok(value);
            }

            return retValue ?? NotFound() ;
        }

        // POST api/movies
        [HttpPost]
        public IHttpActionResult Post([FromBody]Movie movie)
        {
            const int YEAR_OF_FIRST_MOVIE = 1896;

            if (movie == null || String.IsNullOrWhiteSpace(movie.Title) || movie.Year < YEAR_OF_FIRST_MOVIE)
            {
                var msg = String.Format("Movie object failed validity checks {0}", (movie == null) ? "deserializtion failure" : "");
                Trace.WriteLine(msg, "POST");
                return BadRequest(msg);
            }

            try
            {
                // Return back response with Url to new content, 
                var id = _repostiory.Store(movie);
                
                // Use CreatedAtRoute specifying a RouteName "GetById" to form Url, otherwise we
                // end up with api/movies/Get/1 <- not "Get" is not relevant to path
                return CreatedAtRoute<Movie>("GetById", new {  id = movie.Id }, movie);
            }
            catch (Exception e)
            {
                var msg = $"Unable to Store object, due to: {e.Message}";
                Trace.WriteLine(msg, "POST");
                return BadRequest(msg);
            }
        }
    }
}
