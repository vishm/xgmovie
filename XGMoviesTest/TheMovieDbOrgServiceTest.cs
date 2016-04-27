using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XGMoviesBackEnd.ExternalServices;

namespace XGMoviesTest
{
    /// <summary>
    /// Summary description for TheMovieDbOrgService
    /// </summary>
    [TestClass]
    public class TheMovieDbOrgServiceTest
    {
        public TheMovieDbOrgServiceTest()
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
        public void GetMovieIdAsync_WhereValidMovieProvided()
        {
            // Arrange
            var service = new TheMovieDbOrgService();

            // Act
            var expectedId = 19995;

            var task = service.GetMovieIdAsync("Avatar", 2009);
            task.Wait();
            var id = task.Result;

            // Assert
            Assert.AreEqual(expectedId, id, "Id mismatch for Avatar");
        }

        [TestMethod]
        public void GetMovieIdAsync_WhereValidMovieWithSpacesProvided()
        {
            // Arrange
            var service = new TheMovieDbOrgService();

            // Act
            var expectedId = 278927;

            var task = service.GetMovieIdAsync("The Jungle Book", 2016);
            task.Wait();
            var id = task.Result;

            // Assert
            Assert.AreEqual(expectedId, id, "Id mismatch for Avatar");
        }

        [TestMethod]
        public void GetMovieIdAsync_WhereInvalidSearch()
        {
            // Arrange
            var service = new TheMovieDbOrgService();

            // Act
            ArgumentException failure = null;
            try
            {
                var task = service.GetMovieIdAsync("Random", 1980);
                task.Wait();
                var id = task.Result;
            }
            catch(AggregateException e) // We're getting the aggregated due to use of task.Wait so unbundle
            {                
                failure = e.InnerExceptions.First() as ArgumentException;
            }

            // Assert
            Assert.IsNotNull(failure, "Exception expected");            
        }
    }
}
