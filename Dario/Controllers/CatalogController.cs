using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;

namespace Dario.Controllers
{
    public class CatalogController:ApiController
    {
        // http://localhost:49430/rest/services
        [Route("rest/services")]
        public HttpResponseMessage GetCatalog()
        {
            var agsConfigDir = ConfigurationManager.AppSettings["AgsConfigDir"];

            // todo read dynamicly from config directory
            var path = agsConfigDir + "catalog.json";
            var content = File.ReadAllText(path);
            var jsonObject = JsonConvert.DeserializeObject(content);
            return Request.CreateResponse(HttpStatusCode.OK, jsonObject);
        }

    }
}