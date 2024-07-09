using Microsoft.Data.SqlClient;

namespace library.Data
{
    public class DatabaseAccess
    {
        private readonly string _connectionString;

        public DatabaseAccess(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<T>> QueryAsync<T>(string sql, Func<SqlDataReader, T> map)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);
            var results = new List<T>();
            connection.Open();
            using var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                results.Add(map(reader));
            }
            return results;
        }

    }
}
