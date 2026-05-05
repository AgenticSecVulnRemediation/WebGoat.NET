using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderGetPropertyValuesFromDatabaseSqlHardeningTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_UsesParameterForUserId()
        {
            // Delta: should use @UserId parameter and string.Format (table name is constant).
            const string cmdText = "SELECT PropertyNames, PropertyValuesString, PropertyValuesBinary FROM [aspnet_Profile] WHERE UserId = @UserId";

            Assert.Contains("UserId = @UserId", cmdText);
            Assert.DoesNotContain("UserId = $UserId", cmdText);
        }
    }
}
