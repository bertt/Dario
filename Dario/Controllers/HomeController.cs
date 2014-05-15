using System;
using System.Web.Http;

namespace Dario.Controllers
{
    public class HomeController : ApiController
    {
        public string GetHome()
        {
            // use this?
            // var cd = AppDomain.CurrentDomain.BaseDirectory;
            
            return "home";
        }
    }
}
