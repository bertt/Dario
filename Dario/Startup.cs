using Microsoft.Owin;
using Microsoft.Owin.Extensions;
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
            app.UseWelcomePage();
            app.UseErrorPage();
            // the next line is needed for handling the UriPathExtensionMapping...
            app.UseStageMarker(PipelineStage.MapHandler);
        }
    }
}
