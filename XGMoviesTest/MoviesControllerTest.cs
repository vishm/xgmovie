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
            var controller = new MoviesController();

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
            var controller = new MoviesController();

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
            var controller = new MoviesController();

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
            var controller = new MoviesController();

            // Act            
            var movie = new Movie() { Title = "Jungle Book", Year = 1967 };
            var response = controller.Post(movie) as CreatedAtRouteNegotiatedContentResult<Movie>;

            // Assert
            Assert.IsNotNull(response, "Unexpected response");
            Assert.IsTrue(response.RouteName == "GetById", "RouteName not as expectd.");        
            Assert.IsTrue(response.Content.Id == (int)response.RouteValues["id"], "mismatch between route value");
        }
    }
}
