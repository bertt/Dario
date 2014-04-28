using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Dario.Controllers
{
    public class TileController : ApiController
    {
        // testurl http://localhost:49430/layers/6/36/39.jpg
        [Route("{layers}/{level:int:min(0)}/{col:int:min(0)}/{row:int:min(0)}.{ext}")]
        public HttpResponseMessage GetTile(string layers, string level, int col, int row,string ext)
        {
            var mbtiledb = @"E:\bertt\skydrive\dev\research\140424_gdal\testdata\geodata.mbtiles";

            var _connectionString = string.Format("Data Source={0}; FailIfMissing=False", mbtiledb);
            var mbTileProvider = new MbTileProvider(_connectionString);
            var image = mbTileProvider.GetTile(level, col, row);
            var httpResponseMessage = new HttpResponseMessage();
            var memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Jpeg);
            httpResponseMessage.Content = new ByteArrayContent(memoryStream.ToArray());
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            httpResponseMessage.StatusCode = HttpStatusCode.OK;            
            return httpResponseMessage;
        }
    }
}

/**
 * 
 * var graphics = Graphics.FromImage(bitmap);
var pen = new Pen(Color.Red) { Width = 6 };
graphics.DrawLine(pen, 0, 0, 200, 200);
graphics.DrawLine(pen, 200, 0, 0, 200);
*/
