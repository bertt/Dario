using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Dario.Controllers
{
    // [Route("bert")]
    // [Route("rest/services/{serviceName}/FeatureServer")]
    public class HomeController : ApiController
    {
        public string GetHome()
        {
            // use this?
            var cd = AppDomain.CurrentDomain.BaseDirectory;
            
            return "home";
        }
    }

    public class TestController : ApiController
    {
        public HttpResponseMessage GetHome()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "hallo");
        }
    }
}
