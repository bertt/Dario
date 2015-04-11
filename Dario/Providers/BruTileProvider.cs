using System;
using System.Net;
using BruTile;
using BruTile.Web;

namespace Dario.Providers
{
    public class BruTileProvider
    {
        public static byte[] GetTile(string layer, string level, int col, int row)
        {
            var tileServer = (KnownTileServers)Enum.Parse(typeof (KnownTileServers), layer,true);
            var tilesource = TileSource.Create(tileServer);
            return GetTile(tilesource, level, col, row);
        }

        private static byte[] GetTile(ITileSource tileSource, string level, int col, int row)
        {
            try
            {
                var tileInfo = new TileInfo();
                var tileIndex = new TileIndex(col, row, level);
                tileInfo.Index = tileIndex;
               
                return tileSource.Provider.GetTile(tileInfo);
            }
            catch (WebException)
            {
                // intented: do nothing
                return null;
            }
        }
    }
}