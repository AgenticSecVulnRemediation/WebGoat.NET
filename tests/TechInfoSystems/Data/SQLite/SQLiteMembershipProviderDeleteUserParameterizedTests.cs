using Mono.Data.Sqlite;
using Xunit;

// Assumption: production code namespace is TechInfoSystems.Data.SQLite
namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserTests
    {
        [Fact]
        public void DeleteUser_UsesNamedParametersNotStringInterpolation()
        {
            // Arrange
            var username = "user";
            var applicationId = "app";

            // Act
            var selectSql = "SELECT UserId FROM [aspnet_Users] WHERE LoweredUsername = @Username AND ApplicationId = @ApplicationId";
            using var selectCmd = new SqliteCommand(selectSql);
            selectCmd.Parameters.AddWithValue("@Username", username);
            selectCmd.Parameters.AddWithValue("@ApplicationId", applicationId);

            var deleteSql = "DELETE FROM [aspnet_Users] WHERE LoweredUsername = @Username AND ApplicationId = @ApplicationId";
            using var deleteCmd = new SqliteCommand(deleteSql);
            deleteCmd.Parameters.AddWithValue("@Username", username);
            deleteCmd.Parameters.AddWithValue("@ApplicationId", applicationId);

            // Assert
            Assert.Contains("@Username", selectCmd.CommandText);
            Assert.Contains("@ApplicationId", selectCmd.CommandText);
            Assert.NotNull(selectCmd.Parameters["@Username"]);
            Assert.NotNull(selectCmd.Parameters["@ApplicationId"]);

            Assert.Contains("@Username", deleteCmd.CommandText);
            Assert.Contains("@ApplicationId", deleteCmd.CommandText);
            Assert.NotNull(deleteCmd.Parameters["@Username"]);
            Assert.NotNull(deleteCmd.Parameters["@ApplicationId"]);
        }
    }
}
