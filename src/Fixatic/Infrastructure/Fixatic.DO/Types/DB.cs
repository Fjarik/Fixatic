using System.Data.SqlClient;

namespace Fixatic.DO.Types
{
    public class DB
    {
        private readonly string _connectionString;

        public DB(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static readonly int IgnoredID = -1;

        public async Task<SqlConnection> ConnectAsync()
        {
            var c = new SqlConnection(_connectionString);
            await c.OpenAsync();
            return c;
        }

        public async Task<SqlDataReader> ExecuteReaderAsync(SqlCommand cmd)
        {
            if (cmd.Connection == null)
                cmd.Connection = await ConnectAsync();
            return await cmd.ExecuteReaderAsync();
        }

        public async Task<int> ExecuteNonQueryAsync(SqlCommand cmd)
        {
            if (cmd.Connection == null)
                cmd.Connection = await ConnectAsync();
            return await cmd.ExecuteNonQueryAsync();
        }

        public async Task<object?> ExecuteScalarAsync(SqlCommand cmd)
        {
            if (cmd.Connection == null)
                cmd.Connection = await ConnectAsync();
            return await cmd.ExecuteScalarAsync();
        }

        public async Task<T?> ExecuteScalarAsync<T>(SqlCommand cmd) where T : class
        {
            if (cmd.Connection == null)
                cmd.Connection = await ConnectAsync();
            return await cmd.ExecuteScalarAsync() as T;
        }
    }
}
