using System;
using System.Drawing;
using System.IO;
using System.Net;
using BruTile;
using BruTile.Web;

namespace Dario.Providers
{
    public class BrutTileProvider
    {
        public static Image GetTile(string lyr, string level, int col, int row)
        {
            var tileServer = (KnownTileServers)Enum.Parse(typeof (KnownTileServers), lyr,true);
            var tilesource = TileSource.Create(tileServer);
            return GetTile(tilesource, level, col, row);
        }


        private static Image GetTile(ITileSource tileSource, string level, int col, int row)
        {
            try
            {
                var tileInfo = new TileInfo();
                var tileIndex = new TileIndex(col, row, level);
                tileInfo.Index = tileIndex;
                var bytes = tileSource.Provider.GetTile(tileInfo);
                var ms = new MemoryStream(bytes);
                var image = Image.FromStream(ms);
                return image;
            }
            catch (WebException)
            {
                // intented: do nothing
                return null;
            }
        }


    }
}