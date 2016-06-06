using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XGMoviesBackEnd.Domain;

namespace XGMoviesBackEnd.DbDataAccess
{
    /// <summary>
    /// Some test/seed data  which we invoke via Configuration.cs
    /// </summary>
    class NewMoviesSeedData
    {
        public void Seed(MoviesContext context)
        {
            if (context.Actors.Count() == 0)
            {
                var actors = ActorDetails();
                context.Actors.AddRange(actors);
                context.SaveChanges();

                context.Movies.AddRange(GetMovies(actors));

                context.SaveChanges();
            }
        }

        private static IEnumerable<Actor> ActorDetails()
        {
            var retValue = new List<Actor>()
            {
                new Actor() { Name = "Billy Crystal" },
                new Actor() { Name = "Yvonne Strahovski" },
                new Actor() { Name = "Robert De Niro" },
            };

            return retValue;
        }

        private static IEnumerable<Movie> GetMovies(IEnumerable<Actor> actors)
        {
            var retValue = new List<Movie>
            {
                new Movie()
                {
                    Title = "Monsters University",
                    Year = 1955,
                    TheMovideDbOrgId = 62211,
                    Characters = new List<MovieCharacter>()
                    {
                        new MovieCharacter() { Name = "Michael \"Mike\" Wazowski (voice)", PlayedBy = LookupActor(actors, "Billy Crystal") }
                    }
                },
                new Movie()
                {
                    Title = "Killer Elite",
                    Year = 1955,
                    TheMovideDbOrgId = 49021,
                    Characters = new List<MovieCharacter>()
                    {
                        new MovieCharacter() { Name = "Anne", PlayedBy = LookupActor(actors, "Yvonne Strahovski") },
                        new MovieCharacter() { Name = "Hunter", PlayedBy = LookupActor(actors, "Robert De Niro") }

                    }
                },
                new Movie()
                {
                        Title = "Analyze This",
                        Year = 1999,
                        TheMovideDbOrgId = 9535,
                        Characters = new List<MovieCharacter>()
                        {
                            new MovieCharacter() { Name = "Paul Vitti", PlayedBy = LookupActor(actors, "Robert De Niro") },
                            new MovieCharacter() { Name = "Dr. Ben Sobel", PlayedBy = LookupActor(actors, "Billy Crystal") }
                        }
                }
            };

            return retValue;
        }

        private static Actor LookupActor(IEnumerable<Actor> actors, string  actorName)
        {            
            var actor = actors.SingleOrDefault(x => String.Compare(x.Name, actorName, StringComparison.InvariantCultureIgnoreCase) == 0);

            System.Diagnostics.Debug.Assert(!String.IsNullOrWhiteSpace(actor.Name));

            return actor;
        }
    }
}
