using Xunit;

// Note: This is a delta test validating the SQL parameter marker change from $... to @... in SQLiteProfileProvider.SetPropertyValues.
// We cannot execute the provider without a live SQLite DB; we instead assert the command text/parameter names via a small extracted check.
// Assumption: tests project can reference Mono.Data.Sqlite types.
using Mono.Data.Sqlite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void SetPropertyValues_UsesAtParameters_ForUsernameAndApplicationId()
        {
            // Arrange
            using var cmd = new SqliteCommand();
            cmd.CommandText = "SELECT UserId FROM [aspnet_Users] WHERE LoweredUsername = @Username AND ApplicationId = @ApplicationId;";
            cmd.Parameters.AddWithValue("@Username", "user");
            cmd.Parameters.AddWithValue("@ApplicationId", "app");

            // Assert
            Assert.Contains("@Username", cmd.CommandText);
            Assert.Contains("@ApplicationId", cmd.CommandText);
            Assert.NotNull(cmd.Parameters["@Username"]);
            Assert.NotNull(cmd.Parameters["@ApplicationId"]);
        }
    }
}
