using Xunit;
using Mono.Data.Sqlite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserParameterizationTests
    {
        [Fact]
        public void DeleteUser_UsesAtParameters_ForUsernameAndApplicationId()
        {
            // Delta: switched from $Username/$ApplicationId to @Username/@ApplicationId in the DELETE statement.
            const string expectedSql = "DELETE FROM [aspnet_Users] WHERE LoweredUsername = @Username AND ApplicationId = @ApplicationId";

            var cmd = new SqliteCommand(expectedSql);
            cmd.Parameters.AddWithValue("@Username", "user");
            cmd.Parameters.AddWithValue("@ApplicationId", "app");

            Assert.Equal(expectedSql, cmd.CommandText);
            Assert.True(cmd.Parameters.Contains("@Username"));
            Assert.True(cmd.Parameters.Contains("@ApplicationId"));
        }
    }
}
