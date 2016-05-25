using System.Web.Mvc;

namespace XGMovies.Areas.Movie
{
    public class MovieAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Movie";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Movie_default",
                "Movie/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}