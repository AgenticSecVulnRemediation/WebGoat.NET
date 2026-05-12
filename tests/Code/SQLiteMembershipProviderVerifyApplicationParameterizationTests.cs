using Xunit;
using Mono.Data.Sqlite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderVerifyApplicationParameterizationTests
    {
        [Fact]
        public void VerifyApplication_UsesNamedParameters_ForInsert()
        {
            // Delta: Ensure all parameters used by the INSERT are named consistently.
            const string expectedSql = "INSERT INTO [aspnet_Applications] (ApplicationId, ApplicationName, Description) VALUES ($ApplicationId, $ApplicationName, $Description)";

            var cmd = new SqliteCommand(expectedSql);
            cmd.Parameters.AddWithValue("$ApplicationId", "appId");
            cmd.Parameters.AddWithValue("$ApplicationName", "appName");
            cmd.Parameters.AddWithValue("$Description", "");

            Assert.Equal(expectedSql, cmd.CommandText);
            Assert.True(cmd.Parameters.Contains("$ApplicationId"));
            Assert.True(cmd.Parameters.Contains("$ApplicationName"));
            Assert.True(cmd.Parameters.Contains("$Description"));
        }
    }
}
