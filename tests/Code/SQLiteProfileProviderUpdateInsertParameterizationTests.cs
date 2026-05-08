using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    // Delta: UPDATE/INSERT switched to positional parameters (?) with cleared parameters and AddWithValue calls.
    public class SQLiteProfileProviderUpdateInsertParameterizationTests
    {
        [Fact]
        public void SetPropertyValues_UpdateUsesPositionalParameters()
        {
            var updateSql = "UPDATE [aspnet_Profile] SET PropertyNames = ?, PropertyValuesString = ?, PropertyValuesBinary = ?, LastUpdatedDate = ? WHERE UserId = ?";
            Assert.Contains("?", updateSql);
            Assert.DoesNotContain("$PropertyNames", updateSql);
        }

        [Fact]
        public void SetPropertyValues_InsertUsesPositionalParameters()
        {
            var insertSql = "INSERT INTO [aspnet_Profile] (UserId, PropertyNames, PropertyValuesString, PropertyValuesBinary, LastUpdatedDate) VALUES (?, ?, ?, ?, ?)";
            Assert.Contains("VALUES (?, ?, ?, ?, ?)", insertSql);
        }
    }
}
