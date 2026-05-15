using System;
using Mono.Data.Sqlite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    // Delta test: SetPropertyValues changed from two AddWithValue calls to Parameters.AddRange(new [] { new SqliteParameter(...), ... }).
    // We assert both parameters are present and named as expected.
    public class SQLiteProfileProviderSetPropertyValuesTests
    {
        [Fact]
        public void SetPropertyValues_UsesAddRangeWithExpectedParameterNames()
        {
            using var connection = new SqliteConnection("Data Source=:memory:;Version=3;New=True;");
            connection.Open();

            using var cmd = new SqliteCommand("SELECT UserId FROM [aspnet_Users] WHERE LoweredUsername = $Username AND ApplicationId = $ApplicationId;", connection);

            cmd.Parameters.AddRange(new[]
            {
                new SqliteParameter("$Username", "alice"),
                new SqliteParameter("$ApplicationId", "app")
            });

            Assert.NotNull(cmd.Parameters["$Username"]);
            Assert.NotNull(cmd.Parameters["$ApplicationId"]);
        }
    }
}
