using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGMoviesBackEnd.Models
{
    /// <summary>
    /// Data access object used to communicate from front to back end
    /// -- Ideally would have seperate public and intenal models and
    /// automap between for segggregation/protection
    /// </summary>
    public class Movie
    {
        public int Id { get; set; }
        public String Title { get; set; }
        public ushort Year { get; set; }
        public int MovieDbId { get; set; }
    }
}
