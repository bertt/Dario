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
            app.UseWebApi(Models.Config.GetHttpConfiguration());
            app.UseWelcomePage();
            app.UseErrorPage();
            // the next line is needed for handling the UriPathExtensionMapping...
            app.UseStageMarker(PipelineStage.MapHandler);
            app.UseStaticFiles();
        }
    }
}


// test with configr:
//var c= Config.Global.LoadScriptFile("config.csx");
//var s=c["builtfor"];
