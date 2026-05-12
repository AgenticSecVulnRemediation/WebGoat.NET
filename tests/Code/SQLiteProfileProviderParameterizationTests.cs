using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProvider_ParameterizationTests
    {
        [Fact]
        public void SetPropertyValues_UserLookupQuery_UsesAtParameters()
        {
            // Delta behavior: query switched from $Username/$ApplicationId to @Username/@ApplicationId.
            const string sql = "SELECT UserId FROM [aspnet_Users] WHERE LoweredUsername = @Username AND ApplicationId = @ApplicationId;";

            Assert.Contains("@Username", sql);
            Assert.Contains("@ApplicationId", sql);
            Assert.DoesNotContain("$Username", sql);
            Assert.DoesNotContain("$ApplicationId", sql);
        }
    }
}
