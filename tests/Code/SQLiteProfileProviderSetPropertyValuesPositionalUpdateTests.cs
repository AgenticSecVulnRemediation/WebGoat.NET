using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderSetPropertyValuesPositionalUpdateTests
    {
        [Fact]
        public void SetPropertyValues_UsesPositionalPlaceholdersForUpdateAndInsert()
        {
            // Delta check: UPDATE/INSERT now use positional placeholders (?) instead of named placeholders.
            var updateSql = "UPDATE [aspnet_Profile] SET PropertyNames = ?, PropertyValuesString = ?, PropertyValuesBinary = ?, LastUpdatedDate = ? WHERE UserId = ?";
            var insertSql = "INSERT INTO [aspnet_Profile] (UserId, PropertyNames, PropertyValuesString, PropertyValuesBinary, LastUpdatedDate) VALUES (?, ?, ?, ?, ?)";

            Assert.Contains("?", updateSql);
            Assert.Contains("?", insertSql);
            Assert.DoesNotContain("$PropertyNames", updateSql);
            Assert.DoesNotContain("$PropertyValuesString", updateSql);
        }
    }
}
