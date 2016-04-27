using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGMoviesBackEnd.ExternalServices
{
    /// <summary>
    /// Representation of movie details as returne within response body when 
    /// issuing /search/movie
    /// </summary>
    class TheMovieDbOrgMovieDetail
    {
        public int Id = 0;
        public string Title = String.Empty;
    }
}
