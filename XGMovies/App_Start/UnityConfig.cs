using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;
using System;
using XGMoviesBackEnd.ExternalServices;
using XGMoviesBackEnd.Repository;
using System.Configuration;

namespace XGMovies
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            RegisterType(container);
            
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }

        private static void RegisterType(UnityContainer container)
        {
            // Initialize our TheMovieDbOrg service for use later
            var apiKey = ConfigurationManager.AppSettings["TheMovieDbOrgApiKey"];
            container.RegisterType<IMovieIDResolutionService, TheMovieDbOrgService>(
                                new ContainerControlledLifetimeManager(),
                                new InjectionConstructor(apiKey));

            // Pair up our movie repository with movie resolution service.
            //var inMemoryRepo = new InMemoryRepository(container.Resolve<IMovieIDResolutionService>(), seed: true);
            var moviesRepo = new DbMovieRepository(container.Resolve<IMovieIDResolutionService>());

            container.RegisterInstance<IMovieRepository>(moviesRepo);
        }
    }
}