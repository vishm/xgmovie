using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XGMoviesBackEnd.Repository;
using System.Collections.Generic;
using XGMoviesBackEnd.Domain;
using Rhino.Mocks;
using XGMoviesBackEnd.ExternalServices;
using System.Threading.Tasks;
using System.Linq;

namespace XGMoviesTest
{
    [TestClass]
    public class MovieRepositoryTest
    {
        [TestMethod]
        public void Store_EnsureMoviePersisted()
        {
            // Arrange            
            var idResolutionStub = MockRepository.GenerateStub<IMovieIDResolutionService>();
            idResolutionStub.Stub(x => x.GetMovieIdAsync(Arg<String>.Is.Anything, Arg<ushort>.Is.Anything))
                            .Return(Task.FromResult(1));
            

            var repository = new InMemoryRepository(idResolutionStub, seed: false);

            // Act            
            var initialCount = repository.GetAllMovies().Count;
            repository.Store(new Movie() { Id = 0, Title = "Movie1", Year = 1900, TheMovideDbOrgId = 0 });
            var postStoreCount = repository.GetAllMovies().Count;
            
            
            // Assert
            Assert.IsTrue(initialCount == 0, "Repository not initialised as empty");
            Assert.IsTrue(postStoreCount == 1, "Single item expected");            
        }

        [TestMethod]
        public void Store_EnsureMoviePersistedWithExternalMovieId()
        {
            // Arrange            
            const int externalMovieId = 27;
            var idResolutionStub = MockRepository.GenerateStub<IMovieIDResolutionService>();
            idResolutionStub.Stub(x => x.GetMovieIdAsync(Arg<String>.Is.Anything, Arg<ushort>.Is.Anything))
                            .Return(Task.FromResult(externalMovieId));


            var repository = new InMemoryRepository(idResolutionStub, seed: false);

            // Act            
            repository.Store(new Movie() { Id = 0, Title = "Movie1", Year = 1900, TheMovideDbOrgId = 0 });
            var peristedMovie = repository.GetAllMovies().First();

            // Assert
            Assert.AreEqual(externalMovieId, peristedMovie.TheMovideDbOrgId, "Single item expected");
        }

        [TestMethod]
        public void Store_EnsureMovieWithIdNotRepersisted()
        {
            // Arrange
            var idResolutionStub = MockRepository.GenerateStub<IMovieIDResolutionService>();
            idResolutionStub.Stub(x => x.GetMovieIdAsync(Arg<String>.Is.Anything, Arg<ushort>.Is.Anything))
                            .Return(Task.FromResult(1));


            var repository = new InMemoryRepository(idResolutionStub, seed: false);

            // Act            
            var initialCount = repository.GetAllMovies().Count;
            repository.Store(new Movie() { Id = 0, Title = "Movie1", Year = 1900, TheMovideDbOrgId = 0 });
            var movie = repository.GetAllMovies().First();
            repository.Store(movie);
            var postStoreCount = repository.GetAllMovies().Count();

            // Assert
            Assert.IsTrue(initialCount == 0, "Repository not initialised as empty");
            Assert.IsTrue(postStoreCount == 1, "Single item expected");
        }

        [TestMethod]
        public void GetAllMovies_EnsureAllMoviesReturned()
        {
            // Arrange
            var idResolutionStub = MockRepository.GenerateStub<IMovieIDResolutionService>();
            idResolutionStub.Stub(x => x.GetMovieIdAsync(Arg<String>.Is.Anything, Arg<ushort>.Is.Anything))
                .Return(Task.FromResult(1));

            var repository = new InMemoryRepository(idResolutionStub, seed: false);

            // Act
            List<Movie> listOfMovies = new List<Movie>
            {
                new Movie() { Id = 0, Title = "Movie1", Year = 1900, TheMovideDbOrgId = 0 },
                new Movie() { Id = 0, Title = "Movie2", Year = 1901, TheMovideDbOrgId = 0 },
            };

            listOfMovies.ForEach(movie => repository.Store(movie));
            var allMovies = repository.GetAllMovies();

            // Assert
            CollectionAssert.AreEquivalent(listOfMovies, allMovies, "Mismatch between expected results");
        }

        [TestMethod]
        public void GetAllMovies_GetExistingMovieById()
        {
            // Arrange
            var idResolutionStub = MockRepository.GenerateStub<IMovieIDResolutionService>();
            idResolutionStub.Stub(x => x.GetMovieIdAsync(Arg<String>.Is.Anything, Arg<ushort>.Is.Anything))
                .Return(Task.FromResult(1));

            var repository = new InMemoryRepository(idResolutionStub, seed: false);

            // Act
            List<Movie> listOfMovies = new List<Movie>
            {
                new Movie() { Id = 0, Title = "Movie1", Year = 1900, TheMovideDbOrgId = 0 },
                new Movie() { Id = 0, Title = "Movie2", Year = 1901, TheMovideDbOrgId = 0 },
                new Movie() { Id = 0, Title = "Movie3", Year = 1901, TheMovideDbOrgId = 0 },
            };

            listOfMovies.ForEach(movie => repository.Store(movie));
            var movieFound = repository.GetMovie(2);

            // Assert
            Assert.IsNotNull(movieFound, "Movie not found as expected");
            Assert.AreEqual(listOfMovies[1].Title, movieFound.Title, false, "Title mismatch");
        }
    }
}
