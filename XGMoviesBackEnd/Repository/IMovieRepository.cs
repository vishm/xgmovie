using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGMoviesBackEnd.Repository
{
    /// <summary>
    /// Respository pattern used to surface up a basic query / store 
    /// persistance mechanism 
    /// </summary>
    public interface IMovieRepository
    {
        /// <summary>
        /// Fetch all movies from the store
        /// </summary>
        /// <returns>Empty or list of movies available</returns>
        List<Models.Movie> GetAllMovies();

        /// <summary>
        /// Fetch a particular movie record by id.
        /// </summary>
        /// <param name="movieDbId"></param>
        /// <returns></returns>
        Models.Movie GetMovie(int movieDbId);

        /// <summary>
        /// Store Movie object into persisant store. Enrich with both an 
        /// internal id and external id
        /// </summary>
        /// <param name="movie">Object to be persisted</param>
        /// <exception cref="ArgumentException">Thrown movie details don't match an external id</exception>
        /// <returns>id of Movie object</returns>
        int Store(Models.Movie movie);
    }
}
