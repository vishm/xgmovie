using System;
using System.Linq;
using System.Web.Http;
using XGMoviesBackEnd.Repository;
using System.Diagnostics;
using AutoMapper;
using System.Collections.Generic;

namespace XGMovies.Controllers
{
    [RoutePrefix("api/movies")]
    public class MoviesController : ApiController
    {
        IMovieRepository _repostiory;
        
        public MoviesController(IMovieRepository movieRepository)
        {
            _repostiory = movieRepository;
        }

        // GET api/movies
        [HttpGet]
        public IHttpActionResult Get()
        {            
            var allMovies = _repostiory.GetAllMovies().AsEnumerable();
            var retData = Mapper.Map<IEnumerable<XGMoviesBackEnd.Domain.Movie>, IEnumerable<Models.Movie>>(allMovies);
            
            return Ok(retData);
        }

        // GET api/movies/5        
        [HttpGet, Route("{id:int}", Name ="GetById")]
        public IHttpActionResult Get(int id)
        {
            IHttpActionResult retValue = null;

            var value = _repostiory.GetMovie(id);            
            if ( value != null)
            {
                var data = Mapper.Map<XGMoviesBackEnd.Domain.Movie, Models.Movie>(value);
                retValue = Ok(data);
            }

            return retValue ?? NotFound() ;
        }

        // POST api/movies
        [HttpPost]
        public IHttpActionResult Post([FromBody]Models.NewMovie movie)
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
                var domainModel = Mapper.Map<Models.NewMovie, XGMoviesBackEnd.Domain.Movie>(movie);
                var id = _repostiory.Store(domainModel);

                var newMovieObj = Mapper.Map<XGMoviesBackEnd.Domain.Movie, Models.Movie>(domainModel);

                // Use CreatedAtRoute specifying a RouteName "GetById" to form Url, otherwise we
                // end up with api/movies/Get/1 <- note "Get" is not relevant to path
                return CreatedAtRoute<Models.Movie>("GetById", new {  id = newMovieObj.ObjectId}, newMovieObj);
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
