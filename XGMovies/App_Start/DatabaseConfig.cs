using System;
using System.Configuration;
using System.Data.Entity;

namespace XGMovies
{
    class DatabaseConfig
    {
        public static void Configure()
        {
            // Note used by to seed database with content when using code first
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<XGMoviesBackEnd.DbDataAccess.MoviesContext,
                                    XGMoviesBackEnd.Migrations.Configuration>());
        }
    }
}
