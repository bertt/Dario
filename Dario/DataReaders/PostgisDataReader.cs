using Dapper;
using Npgsql;

namespace Dario.DataReaders
{
    public static class PostgisDataReader
    {
        public static void ReadPostgis(string connectionString, string sql)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var res = conn.Query(sql);
                foreach (var p in res)
                {
                    // todo: create something here

                }
            }
        }
    }
}