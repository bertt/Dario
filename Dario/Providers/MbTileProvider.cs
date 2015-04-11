using System.Data.SQLite;

namespace Dario.Providers
{
    public class MbTileProvider
    {
        private readonly string _connectionString;

        public MbTileProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public byte[] GetTile(string level, int col, int row)
        {
            var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            try
            {
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText =
                        "SELECT [tile_data] FROM [tiles] WHERE zoom_level = @zoom AND tile_column = @col AND tile_row = @row";
                    command.Parameters.Add(new SQLiteParameter("zoom", level));
                    command.Parameters.Add(new SQLiteParameter("col", col));
                    command.Parameters.Add(new SQLiteParameter("row", row));
                    var tileObj = command.ExecuteScalar();
                    if (tileObj == null) return null;
                    return (byte[])tileObj;
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}