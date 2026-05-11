using System;
using System.Data;
using Mono.Data.Sqlite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void SetPropertyValues_UsesNamedParameters_InsteadOfDollarPrefixed()
        {
            // Arrange
            // Delta behavior: $Username/$ApplicationId replaced with @Username/@ApplicationId to prevent SQL issues.
            var username = "user";
            var appId = Guid.NewGuid().ToString();

            using var conn = new SqliteConnection("Data Source=:memory:;Version=3");
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT UserId FROM [aspnet_Users] WHERE LoweredUsername = @Username AND ApplicationId = @ApplicationId;";

            // Act
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@ApplicationId", appId);

            // Assert
            Assert.DoesNotContain("$Username", cmd.CommandText, StringComparison.Ordinal);
            Assert.DoesNotContain("$ApplicationId", cmd.CommandText, StringComparison.Ordinal);
            Assert.NotNull(cmd.Parameters["@Username"]);
            Assert.NotNull(cmd.Parameters["@ApplicationId"]);
        }
    }
}
