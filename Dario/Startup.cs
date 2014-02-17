using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Dario.Models;

[assembly: OwinStartup(typeof(Dario.Startup))]
namespace Dario
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseWebApi(Config.GetHttpConfiguration());
        }
    }
}
