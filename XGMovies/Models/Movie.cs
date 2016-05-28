using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XGMovies.Models
{
    /// <summary>
    /// Movie model as exposed to public
    /// </summary>
    public class Movie
    {
        public int ObjectId { get; set; }
        public String Title { get; set; }
        public ushort Year { get; set; }
        public int MovieDbId { get; set; }
    }
}