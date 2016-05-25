using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XGMovies.Controllers;
using System.Net.Http;
using XGMoviesBackEnd.Models;
using Newtonsoft.Json;
using System.Linq;
using System.Web.Http.Results;
using XGMoviesBackEnd.Repository;
using XGMoviesBackEnd.ExternalServices;

namespace XGMoviesTest
{
    /// <summary>
    /// Summary description for MovieControllerTest
    /// </summary>
    [TestClass]
    public class MoviesControllerTest
    {
        public MoviesControllerTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void GetShouldReturnEmptyList()
        {
            // Arrange
            var controller = CreeatTestMoviesController();

            // Act            
            var response = controller.Get() as OkNegotiatedContentResult<IEnumerable<Movie>>;
            
            // Assert
            Assert.IsNotNull(response, "Unexpected response");
            Assert.IsTrue(response.Content.Count() == 0, "Expected no content");
        }

        [TestMethod]
        public void PostInvalidNullMovie()
        {
            // Arrange
            var controller = CreeatTestMoviesController();

            // Act            
            var movie = new Movie() { Title = "Jungle Book" };

            var response = controller.Post(null) as BadRequestErrorMessageResult;

            // Assert
            Assert.IsNotNull(response, "Unexpected response");            
            Assert.IsTrue(response.Message.Length != 0, "Expected error message");
        }

        [TestMethod]
        public void PostInvalidMovieState()
        {
            // Arrange
            var controller = CreeatTestMoviesController();

            // Act            
            var movie = new Movie() { Title = "Jungle Book" }; // missing year
            var response = controller.Post(null) as BadRequestErrorMessageResult;

            // Assert
            Assert.IsNotNull(response, "Unexpected response");
            Assert.IsTrue(response.Message.Length != 0, "Expected error message");
        }

        [TestMethod]
        public void PostValidMovieState()
        {
            // Arrange
            var controller = CreeatTestMoviesController();

            // Act            
            var movie = new Movie() { Title = "Jungle Book", Year = 1967 };
            var response = controller.Post(movie) as CreatedAtRouteNegotiatedContentResult<Movie>;

            // Assert
            Assert.IsNotNull(response, "Unexpected response");
            Assert.IsTrue(response.RouteName == "GetById", "RouteName not as expectd.");        
            Assert.IsTrue(response.Content.Id == (int)response.RouteValues["id"], "mismatch between route value");
        }

        [TestMethod]
        public void GetAllMovies()
        {
            // Arrange
            var controller = CreeatTestMoviesController();

            // Act      
            var movies = new[]
            {
                new Movie() { Title = "Jungle Book", Year = 1967 },
                new Movie() { Title = "Deadpool ", Year = 2016 }
            };    
            
            controller.Post(movies[0]);
            var response = controller.Post(movies[1]) as CreatedAtRouteNegotiatedContentResult<Movie>;
            var getAll = controller.Get() as OkNegotiatedContentResult<IEnumerable<Movie>>;

            // Assert
            Assert.IsNotNull(response, "Unexpected response");
            Assert.IsTrue(response.RouteName == "GetById", "RouteName not as expectd.");
            Assert.IsTrue(response.Content.Id == (int)response.RouteValues["id"], "mismatch between route value");
            Assert.AreEqual(movies.Length, getAll.Content.Count(), "Incorrect number of movies retrieved");
        }

        [TestMethod]
        public void GetMovieById()
        {
            // Arrange
            var controller = CreeatTestMoviesController();

            // Act      
            var movies = new[]
            {
                new Movie() { Title = "Jungle Book", Year = 1967 },
                new Movie() { Title = "Deadpool ", Year = 2016 }
            };

            controller.Post(movies[0]);
            var createdItem = controller.Post(movies[1]) as CreatedAtRouteNegotiatedContentResult<Movie>;
            var getById = controller.Get(createdItem.Content.Id) as OkNegotiatedContentResult<Movie>;

            // Assert
            Assert.IsNotNull(getById, "Unexpected response");
            Assert.IsTrue(getById.Content.Id == (int)createdItem.RouteValues["id"], "mismatch between route value");
        }

        [TestMethod]
        public void GetMovieByInvaildId()
        {
            // Arrange
            var controller = CreeatTestMoviesController();

            // Act      
            var movies = new[]
            {
                new Movie() { Title = "Jungle Book", Year = 1967 },
                new Movie() { Title = "Deadpool ", Year = 2016 }
            };

            controller.Post(movies[0]);
            var createdItem = controller.Post(movies[1]) as CreatedAtRouteNegotiatedContentResult<Movie>;
            var getById = controller.Get(999) as NotFoundResult;

            
            // Assert
            Assert.IsNotNull(getById, "Bad Request Expected");
        }

        private static MoviesController CreeatTestMoviesController()
        {
            var m = new InMemoryRepository(new TheMovieDbOrgService(), seed: false);
            var controller = new MoviesController(m);

            return controller;
        }
    }
}
