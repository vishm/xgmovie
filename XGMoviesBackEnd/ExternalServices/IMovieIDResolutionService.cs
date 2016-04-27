using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGMoviesBackEnd.ExternalServices
{
    /// <summary>
    /// Interface that abtracts a service that will provide a unique movie Id
    /// based on movie title and year provided.
    /// </summary>
    public interface IMovieIDResolutionService
    {
        /// <summary>
        /// Lookup/resolve movies id using {title, year} lookup
        /// </summary>
        /// <param name="title"></param>
        /// <param name="year"></param>
        /// <exception cref="ArgumentException">Raised when unable to find id of movie that matches title and year</exception>
        /// <returns></returns>
        Task<int> GetMovieIdAsync(String title, ushort year);
    }
}
