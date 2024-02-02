using System.Data;
using Microsoft.Data.Sqlite;

public static class TodoDatabaseService
{
    public static IDbConnection GetConnection()
    {
        IDbConnection connection = new SqliteConnection("Data Source=app.db");
        connection.Open();

        if (!TableExists(connection))
        {
            CreateTable(connection);
        }

        return connection;
    }

    private static bool TableExists(IDbConnection connection)
    {
        using (var command = connection.CreateCommand())
        {
            command.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='TodoItems';";
            var result = command.ExecuteScalar();

            return result != null;
        }
    }

    private static void CreateTable(IDbConnection connection)
    {
        using (var command = connection.CreateCommand())
        {
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS TodoItems (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    IsCompleted INTEGER NOT NULL DEFAULT 0
                );";
            command.ExecuteNonQuery();
        }
    }
}

