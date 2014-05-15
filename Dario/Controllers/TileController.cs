using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using BruTile.Web;
using Dario.Models;
using Dario.Providers;

namespace Dario.Controllers
{
    public class TileController : ApiController
    {
        // testurl http://localhost:49430/stamentoner,countries,cities/6/36/39.jpg
        [Route("{layers}/{level:int:min(0)}/{col:int:min(0)}/{row:int:min(0)}.{ext}")]
        public HttpResponseMessage GetTile(string layers, string level, int col, int row,string ext)
        {
            var mbtiledir = ConfigurationManager.AppSettings["MbTileDir"];
            // MbTilesTileSource.
            var lyrs = layers.Split(',');
             var images = GetTileImages(mbtiledir,lyrs,level,col,row);

            if (images.Count > 0)
            {
                var resultimage = ImageMerger.Merge(images);

                if (resultimage != null)
                {
                    var bytes = ImageConvertor.Convert(resultimage, ext);
                    var contentType = getMediaType(ext);
                    return GetHttpResponseMessage(bytes, contentType, HttpStatusCode.OK);
                }
            }
            return new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound };
        }

        private List<Image> GetTileImages(string mbtiledir, IEnumerable<string> lyrs, string level, int col, int row)
        {
            var images = new List<Image>();
            Image image=null;
            foreach (var lyr in lyrs)
            {
                KnownTileServers c;
                if(Enum.TryParse(lyr, true, out c))
                {
                    image= BrutTileProvider.GetTile(lyr, level, col, row);
                }
                else
                {
                    var mbtiledb = mbtiledir + getDataSource(lyr);
                    if (File.Exists(mbtiledb))
                    {
                        // todo: hoe werkt dit?
                        var ymax = 1 << Int32.Parse(level);
                        var y = ymax - row - 1;
 

                        var connectionString = string.Format("Data Source={0}", mbtiledb);
                        var mbTileProvider = new MbTileProvider(connectionString);
                        image = mbTileProvider.GetTile(level, col, y);
                    }
                }

                if (image != null)
                {
                    images.Add(image);
                }

            }
            return images;
        } 


        internal HttpResponseMessage GetHttpResponseMessage(byte[] content, string contentType, HttpStatusCode code)
        {
            var httpResponseMessage = new HttpResponseMessage {Content = new ByteArrayContent(content)};
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            httpResponseMessage.StatusCode = HttpStatusCode.OK;
            return httpResponseMessage;
            
        }

        private string getMediaType(string ext)
        {
            var mediatype = "image/jpeg";
            switch (ext)
            {
                case "gif":
                    mediatype = "image/gif";
                    break;
                case "png":
                    mediatype = "image/png";
                    break;
            }
            return mediatype;

        }

        private string getDataSource(string lyr)
        {
            return lyr + ".mbtiles";
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
