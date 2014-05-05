﻿using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Dario.Models;
using Dario.Providers;

namespace Dario.Controllers
{
    public class TileController : ApiController
    {
        // testurl http://localhost:49430/countries,cities/6/36/39.jpg
        [Route("{layers}/{level:int:min(0)}/{col:int:min(0)}/{row:int:min(0)}.{ext}")]
        public HttpResponseMessage GetTile(string layers, string level, int col, int row,string ext)
        {
            var mbtiledir = @"E:\dev\git\bertt\Dario\Dario\data\mbtiles\";
            var lyrs = layers.Split(',');
            var images = new List<Image>();

            foreach (var lyr in lyrs)
            {
                var mbtiledb = getDataSource(lyr);
                var connectionString = string.Format("Data Source={0}", mbtiledir + mbtiledb);
                var mbTileProvider = new MbTileProvider(connectionString);
                var image = mbTileProvider.GetTile(level, col, row);
                images.Add(image);
            }

            var resultimage = ImageMerger.Merge(images);

            if (resultimage != null)
            {
                var bytes = ImageConvertor.Convert(resultimage, ext);
                var contentType = getMediaType(ext);
                return GetHttpResponseMessage(bytes, contentType, HttpStatusCode.OK);
            }
            return new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound };
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
