using System.Web.Http;

namespace Dario.Controllers
{
    public class VersionController:ApiController
    {
        public string GetVersion()
        {
            return "version 0.1";
        }
    }
}