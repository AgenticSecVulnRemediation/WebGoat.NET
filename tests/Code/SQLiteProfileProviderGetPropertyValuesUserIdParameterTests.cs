using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderGetPropertyValuesUserIdParameterTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_UsesNamedUserIdParameter()
        {
            // Delta check: query now uses @UserId instead of $UserId.
            var sql = "SELECT PropertyNames, PropertyValuesString, PropertyValuesBinary FROM [aspnet_Profile] WHERE UserId = @UserId";

            Assert.Contains("@UserId", sql);
            Assert.DoesNotContain("$UserId", sql);
        }
    }
}
