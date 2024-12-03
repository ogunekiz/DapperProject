using Microsoft.Data.Sqlite;
using System.Data;

namespace DapperProject.API.DataContext
{
    public class DapperDbContext
    {
        private readonly string _connectionString;

        public DapperDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
           => new SqliteConnection(_connectionString);

        public void InitializeDatabase()
        {
            using var connection = CreateConnection();
            connection.Open();

            var createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Products (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Price REAL NOT NULL
                )";

            using var command = connection.CreateCommand();
            command.CommandText = createTableQuery;
            command.ExecuteNonQuery();
        }

    }
}
