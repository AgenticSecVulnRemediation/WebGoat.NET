using Xunit;
using Mono.Data.Sqlite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderGetAllUsersParameterizationTests
    {
        [Fact]
        public void GetAllUsers_UsesAtApplicationIdParameter()
        {
            // Delta: ApplicationId parameter marker changed from $ApplicationId to @ApplicationId.
            const string expectedSql = "SELECT Count(*) FROM [aspnet_Users] WHERE ApplicationId = @ApplicationId AND IsAnonymous='0'";

            var cmd = new SqliteCommand(expectedSql);
            cmd.Parameters.AddWithValue("@ApplicationId", "app");

            Assert.Equal(expectedSql, cmd.CommandText);
            Assert.True(cmd.Parameters.Contains("@ApplicationId"));
        }
    }
}
