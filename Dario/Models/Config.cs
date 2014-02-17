using System.Web.Http;

namespace Dario.Models
{
    public static class Config
    {
        public static HttpConfiguration GetHttpConfiguration()
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                "Home", //Route name 
                "api", //URL with parameters 
                new { controller = "Home" } //Parameter defaults 
            );

            return config;
        }
    }
}