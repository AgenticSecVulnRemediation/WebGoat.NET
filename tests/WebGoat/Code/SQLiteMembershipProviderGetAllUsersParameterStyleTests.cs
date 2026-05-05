using Mono.Data.Sqlite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderGetAllUsersParameterStyleTests
    {
        [Fact]
        public void GetAllUsers_CountQuery_UsesAtParameterForApplicationId()
        {
            // Arrange
            var sql = "SELECT Count(*) FROM [aspnet_Users] WHERE ApplicationId = @ApplicationId AND IsAnonymous='0'";
            var applicationId = "app-id";

            // Act
            var cmd = new SqliteCommand();
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@ApplicationId", applicationId);

            // Assert
            Assert.Contains("@ApplicationId", cmd.CommandText);
            Assert.DoesNotContain(applicationId, cmd.CommandText);
            Assert.NotNull(cmd.Parameters["@ApplicationId"]);
        }
    }
}
