using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderSetPropertyValuesParameterPrefixTests
    {
        [Fact]
        public void SetPropertyValues_UserLookup_UsesAtPrefixedNamedParameters()
        {
            // Delta check: provider switched from $Username/$ApplicationId to @Username/@ApplicationId.
            var sql = "SELECT UserId FROM [aspnet_Users] WHERE LoweredUsername = @Username AND ApplicationId = @ApplicationId;";

            Assert.Contains("@Username", sql);
            Assert.Contains("@ApplicationId", sql);
            Assert.DoesNotContain("$Username", sql);
            Assert.DoesNotContain("$ApplicationId", sql);
        }
    }
}
