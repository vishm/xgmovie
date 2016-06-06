using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGMoviesBackEnd.DbDataAccess
{
    public class MoviesContext : DbContext
    {
        public MoviesContext() : this("MoviesDb")
        {
        }

        public MoviesContext(String connectionString) : base(connectionString)
        {
            
        }

        public DbSet<XGMoviesBackEnd.Domain.Movie>  Movies { get; set; }
        public DbSet<XGMoviesBackEnd.Domain.Actor> Actors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {            
            base.OnModelCreating(modelBuilder);
        }

        
    }
}
