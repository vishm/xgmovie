using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;
using System;
using XGMoviesBackEnd.ExternalServices;
using XGMoviesBackEnd.Repository;

namespace XGMovies
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            RegisterType(container);
            
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }

        private static void RegisterType(UnityContainer container)
        {
            container.RegisterType<IMovieIDResolutionService, TheMovieDbOrgService>(new ContainerControlledLifetimeManager());
            var inMemoryRepo = new InMemoryRepository(container.Resolve<IMovieIDResolutionService>(), seed: true);
            container.RegisterInstance<IMovieRepository>(inMemoryRepo);
        }
    }
}