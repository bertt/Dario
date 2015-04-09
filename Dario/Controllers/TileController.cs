using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BruTile.Web;
using Dario.Models;
using Dario.Providers;

namespace Dario.Controllers
{
    public class TileController : ApiController
    {
        readonly ImageFetcher _imageFetcher = new ImageFetcher();

        // testurl http://localhost:49430/stamentoner,countries,cities/6/36/39.jpg
        [Route("{layers}/{level:int:min(0)}/{col:int:min(0)}/{row:int:min(0)}.{ext}")]
        public HttpResponseMessage GetTile(string layers, string level, int col, int row,string ext)
        {
            var lyrs = layers.Split(',');
             var images = GetTileImages(lyrs,level,col,row);

            if (images.Count > 0)
            {
                var resultimage = ImageMerger.Merge(images);

                if (resultimage != null)
                {
                    var bytes = ImageConvertor.Convert(resultimage, ext);
                    var contentType = getMediaType(ext);
                    return JsonResponseMessage.GetHttpResponseMessage(bytes, contentType, HttpStatusCode.OK);
                }
            }
            return new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound };
        }

        private List<Image> GetTileImages(IEnumerable<string> lyrs, string level, int col, int row)
        {
            var images = new List<Image>();
            foreach (var lyr in lyrs)
            {
                var image = FindInAvailableTileSources(lyr, level, col, row);
                
                if (image != null)
                {
                    images.Add(image);
                } 
            }
            return images;
        }

        private Image FindInAvailableTileSources(string lyr, string level, int col, int row)
        {
            return FindInKnownTileSources(lyr, level, col, row)
                   ?? FindConfiguredTileSources(lyr, level, col, row)
                   ?? FindInMbTilesTileSources(lyr, level, col, row);
        }

        private Image FindConfiguredTileSources(string lyr, string level, int col, int row)
        {
            var urlTemplate = ConfigurationManager.AppSettings[lyr];
            return urlTemplate == null ? null : _imageFetcher.Fetch(col, row, level, urlTemplate).Result;
        }

        private Image FindInMbTilesTileSources(string lyr, string level, int col, int row)
        {
            var mbtiledir = ConfigurationManager.AppSettings["MbTileDir"];

            var mbtiledb = mbtiledir + getDataSource(lyr);
            if (!File.Exists(mbtiledb)) return null;

            // todo: hoe werkt dit?
            var ymax = 1 << Int32.Parse(level);
            var y = ymax - row - 1;
            
            var connectionString = string.Format("Data Source={0}", mbtiledb);
            var mbTileProvider = new MbTileProvider(connectionString);
            return mbTileProvider.GetTile(level, col, y);
        }

        private static Image FindInKnownTileSources(string lyr, string level, int col, int row)
        {
            if (EnumIsDefined< KnownTileServers>(lyr, true))
            {
                return BrutTileProvider.GetTile(lyr, level, col, row);
            }
            return null;
        }

        static bool EnumIsDefined<T>(string value, bool ignoreCase = false) where T : struct
        {
            if (ignoreCase)
            {
                T outParameter;
                return (Enum.TryParse(value, true, out outParameter));
            }
            return Enum.IsDefined(typeof(T), value);
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
