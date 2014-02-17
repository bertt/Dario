using System.Web.Http;

namespace Dario.Controllers
{
    public class HomeController : ApiController
    {
        public string GetHome()
        {
            return "home";
        }
    }
}
