using System.Web.Http;
using Dario.Formatters;

namespace Dario.Models
{
    public static class Config
    {
        public static HttpConfiguration GetHttpConfiguration()
        {
            var config = new HttpConfiguration();
            var formatters = config.Formatters;
            formatters.Remove(formatters.XmlFormatter);
            formatters.Add(new PngMediaTypeFormatter());
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional});
            config.Routes.MapHttpRoute("DefaultApiWithExt", "api/{controller}.{ext}");
            config.Routes.MapHttpRoute("Home", "api", new { controller = "Home" });
            return config;
        }
    }
}