using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests_DeleteProfileParameterMarkers
    {
        [Fact]
        public void DeleteProfile_UsesAtParameterMarkers_InsteadOfDollarMarkers()
        {
            // Patch change: $Username/$ApplicationId/$UserId replaced with @Username/@ApplicationId/@UserId.
            const string selectSql = "SELECT UserId FROM [aspnet_Users] WHERE LoweredUsername = @Username AND ApplicationId = @ApplicationId";
            const string deleteSql = "DELETE FROM [aspnet_Profile] WHERE UserId = @UserId";

            Assert.Contains("@Username", selectSql);
            Assert.Contains("@ApplicationId", selectSql);
            Assert.Contains("@UserId", deleteSql);

            Assert.DoesNotContain("$Username", selectSql);
            Assert.DoesNotContain("$UserId", deleteSql);
        }
    }
}
