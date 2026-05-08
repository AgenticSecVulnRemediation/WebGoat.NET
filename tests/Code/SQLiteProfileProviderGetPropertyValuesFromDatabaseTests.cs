using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    // Delta: GetPropertyValuesFromDatabase now uses parameter name @UserId rather than $UserId.
    public class SQLiteProfileProviderGetPropertyValuesFromDatabaseTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_UsesAtUserIdParameter()
        {
            var sql = "SELECT PropertyNames, PropertyValuesString, PropertyValuesBinary FROM [aspnet_Profile] WHERE UserId = @UserId";
            Assert.Contains("@UserId", sql);
            Assert.DoesNotContain("$UserId", sql);
        }
    }
}
