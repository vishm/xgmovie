using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGMovies.Models
{
    /// <summary>
    /// User domain object for creating new movie entry
    /// </summary>
    public class NewMovie
    {
        public String Title { get; set; }
        public ushort Year { get; set; }
    }
}
