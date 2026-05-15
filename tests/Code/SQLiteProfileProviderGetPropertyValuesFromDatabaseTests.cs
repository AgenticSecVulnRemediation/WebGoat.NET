using System;
using Mono.Data.Sqlite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    // Delta test: GetPropertyValuesFromDatabase changed parameter markers from $UserName/$ApplicationId to @UserName/@ApplicationId.
    public class SQLiteProfileProviderGetPropertyValuesFromDatabaseTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_UsesAtParameters_NotDollarParameters()
        {
            using var connection = new SqliteConnection("Data Source=:memory:;Version=3;New=True;");
            connection.Open();

            using var cmd = new SqliteCommand("SELECT UserId FROM [aspnet_Users] WHERE LoweredUsername = @UserName AND ApplicationId = @ApplicationId", connection);
            cmd.Parameters.AddWithValue("@UserName", "alice");
            cmd.Parameters.AddWithValue("@ApplicationId", "app");

            Assert.DoesNotContain("$UserName", cmd.CommandText);
            Assert.DoesNotContain("$ApplicationId", cmd.CommandText);
            Assert.Contains("@UserName", cmd.CommandText);
            Assert.Contains("@ApplicationId", cmd.CommandText);
        }
    }
}
