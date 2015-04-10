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
        public HttpResponseMessage GetTile(string layers, string level, int col, int row, string ext)
        {
            var images = GetTileImages(layers.Split(','), level, col, row);

            if (images.Count > 0)
            {
                var resultimage = ImageMerger.Merge(images);

                if (resultimage != null)
                {
                    var bytes = ImageConvertor.Convert(resultimage, ext);
                    var contentType = GetMediaType(ext);
                    return JsonResponseMessage.GetHttpResponseMessage(bytes, contentType, HttpStatusCode.OK);
                }
            }
            return new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound };
        }

        private List<Image> GetTileImages(IEnumerable<string> layers, string level, int col, int row)
        {
            var images = new List<Image>();
            foreach (var layer in layers)
            {
                var image = FindInAvailableTileSources(layer, level, col, row);

                if (image != null)
                {
                    images.Add(image);
                }
            }
            return images;
        }

        private Image FindInAvailableTileSources(string layer, string level, int col, int row)
        {
            return FindInKnownTileSources(layer, level, col, row)
                   ?? FindConfiguredTileSources(layer, level, col, row)
                   ?? FindInMbTilesTileSources(layer, level, col, row);
        }

        private Image FindConfiguredTileSources(string layer, string level, int col, int row)
        {
            var urlTemplate = ConfigurationManager.AppSettings[layer];
            return urlTemplate == null ? null : _imageFetcher.Fetch(col, row, level, urlTemplate).Result;
        }

        private static Image FindInMbTilesTileSources(string layer, string level, int col, int row)
        {
            var mbtiledir = ConfigurationManager.AppSettings["MbTileDir"];

            var mbtiledb = mbtiledir + GetDataSource(layer);
            if (!File.Exists(mbtiledb)) return null;

            // todo: hoe werkt dit?
            var ymax = 1 << Int32.Parse(level);
            var y = ymax - row - 1;

            var connectionString = string.Format("Data Source={0}", mbtiledb);
            var mbTileProvider = new MbTileProvider(connectionString);
            return mbTileProvider.GetTile(level, col, y);
        }

        private static Image FindInKnownTileSources(string layer, string level, int col, int row)
        {
            if (EnumIsDefined<KnownTileServers>(layer, true))
            {
                return BruTileProvider.GetTile(layer, level, col, row);
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

        private static string GetMediaType(string ext)
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

        private static string GetDataSource(string layer)
        {
            return layer + ".mbtiles";
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
