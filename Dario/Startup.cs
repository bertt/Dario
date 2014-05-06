using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.Hosting;
using Owin;

[assembly: OwinStartup(typeof(Dario.Startup))]
namespace Dario
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseWebApi(Models.Config.GetHttpConfiguration());
            app.UseErrorPage();
            // the next line is needed for handling the UriPathExtensionMapping...
            app.UseStageMarker(PipelineStage.MapHandler);
            app.UseStaticFiles();
            app.UseFileServer(true);   
        }
    }
}
