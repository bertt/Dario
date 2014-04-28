using System.Data.SQLite;
using System.Drawing;
using System.IO;

namespace Dario
{
    public class MbTileProvider
    {
        private string _connectionString;

        public MbTileProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Image GetTile(string Level, int Col, int Row)
        {
            var _connection = new SQLiteConnection(_connectionString);
            _connection.Open();
            Image image;
            using (var command = new SQLiteCommand(_connection))
            {
                command.CommandText = "SELECT [tile_data] FROM [tiles] WHERE zoom_level = @zoom AND tile_column = @col AND tile_row = @row";
                command.Parameters.Add(new SQLiteParameter("zoom", Level));
                command.Parameters.Add(new SQLiteParameter("col", Col));
                command.Parameters.Add(new SQLiteParameter("row", Row));
                var tileObj = command.ExecuteScalar();
                var stream = new MemoryStream((byte[])tileObj);
                var bitmap = new Bitmap(stream);
                image = bitmap;
            }
            _connection.Close();
            return image;
        }
    }
}