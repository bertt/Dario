﻿using System.Web.Http;

namespace Dario.Models
{
    public static class Config
    {
        public static HttpConfiguration GetHttpConfiguration()
        {
            var config = new HttpConfiguration();
            var formatters = config.Formatters;
            formatters.Remove(formatters.XmlFormatter);
            config.Routes.MapHttpRoute("DefaultApiWithExt", "api/{controller}.{ext}/{id}", new { id = RouteParameter.Optional });
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            config.Routes.MapHttpRoute("Home", "api", new { controller = "Home" });
            config.MapHttpAttributeRoutes();
            return config;
        }
    }
}