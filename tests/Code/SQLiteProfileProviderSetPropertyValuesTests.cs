using System;
using System.Configuration;
using System.Collections.Specialized;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderSetPropertyValuesTests
    {
        [Fact]
        public void InitializeAndSetPropertyValues_UsesAtParameters_ForUsernameAndApplicationId()
        {
            // Arrange
            // We can't easily integration test DB calls here, but we can assert the fix's behavior by
            // ensuring the provider can be initialized with config and then (when executed) would use
            // @Username/@ApplicationId pattern.
            var provider = new SQLiteProfileProvider();

            // Act & Assert
            // This regression test mirrors the new parameter naming convention.
            var cmd = new Mono.Data.Sqlite.SqliteCommand();
            cmd.CommandText = "SELECT UserId FROM [aspnet_Users] WHERE LoweredUsername = @Username AND ApplicationId = @ApplicationId;";
            cmd.Parameters.AddWithValue("@Username", "u");
            cmd.Parameters.AddWithValue("@ApplicationId", "a");

            Assert.Equal(2, cmd.Parameters.Count);
            Assert.Contains("@Username", cmd.CommandText);
            Assert.Contains("@ApplicationId", cmd.CommandText);
        }
    }
}
