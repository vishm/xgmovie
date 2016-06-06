using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGMoviesBackEnd.Domain
{
    /// <summary>
    /// Data access object used to communicate from front to back end
    /// -- Ideally would have seperate public and intenal models and
    /// automap between for segggregation/protection
    /// </summary>
    public class Movie
    {
        [Key]
        public int Id { get; set; } 
        public String Title { get; set; }
        public ushort Year { get; set; }
        public int TheMovideDbOrgId { get; set; }  // MovieId from themoviedb.org
        public virtual ICollection<MovieCharacter> Characters { get; set; } // Note use of ICollection of lazy loading        
    }
}
