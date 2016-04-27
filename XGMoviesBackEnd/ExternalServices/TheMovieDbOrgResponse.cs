using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGMoviesBackEnd.ExternalServices
{
    /// <summary>
    /// Representation of main response body from TheMovieDbOrg for /search/movie
    /// </summary>
    class TheMovieDbOrgSearchMovieResponse
    {
        public int page = 0;
        public IEnumerable<TheMovieDbOrgMovieDetail> results = null;
        public int total_results = 0;
        public int total_pages = 0;
    }
}
