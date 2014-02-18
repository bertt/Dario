using ConfigR;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Owin;

[assembly: OwinStartup(typeof(Dario.Startup))]
namespace Dario
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var c= Config.Global.LoadScriptFile("config.csx");
            var s=c["builtfor"];
            app.UseWebApi(Dario.Models.Config.GetHttpConfiguration());
            app.UseWelcomePage();
            app.UseErrorPage();
            // the next line is needed for handling the UriPathExtensionMapping...
            app.UseStageMarker(PipelineStage.MapHandler);
        }
    }
}
