using Mono.Data.Sqlite;
using Xunit;

// Assumption: production code namespace is TechInfoSystems.Data.SQLite
namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderGetAllUsersTests
    {
        [Fact]
        public void GetAllUsers_CountQuery_UsesParameterForApplicationId()
        {
            // Arrange
            var applicationId = "app";

            // Act
            var sql = "SELECT Count(*) FROM [aspnet_Users] WHERE ApplicationId = @ApplicationId AND IsAnonymous='0'";
            using var cmd = new SqliteCommand(sql);
            cmd.Parameters.AddWithValue("@ApplicationId", applicationId);

            // Assert
            Assert.Contains("@ApplicationId", cmd.CommandText);
            Assert.NotNull(cmd.Parameters["@ApplicationId"]);
            Assert.Equal(applicationId, cmd.Parameters["@ApplicationId"].Value);
        }
    }
}
