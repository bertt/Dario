using Microsoft.Owin;
using Owin;
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
