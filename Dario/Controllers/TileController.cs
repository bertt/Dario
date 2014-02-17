using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Dario.Controllers
{
    public class TileController : ApiController
    {
        public HttpResponseMessage GetTile()
        {
            var bitmap = new Bitmap(200, 200, PixelFormat.Format32bppArgb);
            var graphics = Graphics.FromImage(bitmap);
            var pen = new Pen(Color.Black);
            pen.Width = 6;
            graphics.DrawLine(pen,0,0,200,200);
            graphics.DrawLine(pen, 200, 0, 0, 200);
            var image = (Image) bitmap;
            return Request.CreateResponse(HttpStatusCode.OK, image);
        }

        [Route("{layers}/{level:int:min(0)}/{col:int:min(0)}/{row:int:min(0)}.{ext}")]
        public HttpResponseMessage GetTile(string layers, string level, int col, int row)
        {
            Image image = null;
            //var image = Tilestacker.Stack(layers, level, col, row);
            return image != null ?
                Request.CreateResponse(HttpStatusCode.OK, image) :
                Request.CreateResponse(HttpStatusCode.NotFound, "No images found");
        }

    }
}
