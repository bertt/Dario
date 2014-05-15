using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Dario.Controllers
{
    public class CatalogController:ApiController
    {
        // http://localhost:49430/rest/services
        [Route("rest/services")]
        public HttpResponseMessage GetCatalog()
        {
            var agsConfigDir = ConfigurationManager.AppSettings["AgsConfigDir"];
            var path = agsConfigDir + "catalog.json";
            var result = Request.CreateResponse(HttpStatusCode.OK);
            var stream = new FileStream(path, FileMode.Open);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return result;
        }

    }
}