using Mono.Data.Sqlite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderSetPropertyValuesTests
    {
        [Fact]
        public void SetPropertyValues_UserLookup_UsesAtPrefixedParameters()
        {
            // Arrange
            // Fix changes parameters from $Username/$ApplicationId to @Username/@ApplicationId.
            string sql = "SELECT UserId FROM [aspnet_Users] WHERE LoweredUsername = @Username AND ApplicationId = @ApplicationId;";

            // Act
            var cmd = new SqliteCommand(sql);
            cmd.Parameters.AddWithValue("@Username", "user".ToLowerInvariant());
            cmd.Parameters.AddWithValue("@ApplicationId", "appId");

            // Assert
            Assert.True(cmd.Parameters.Contains("@Username"));
            Assert.True(cmd.Parameters.Contains("@ApplicationId"));
            Assert.False(cmd.Parameters.Contains("$Username"));
            Assert.False(cmd.Parameters.Contains("$ApplicationId"));
        }
    }
}
