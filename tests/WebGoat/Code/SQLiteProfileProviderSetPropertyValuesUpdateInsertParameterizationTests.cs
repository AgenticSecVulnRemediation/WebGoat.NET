using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderSetPropertyValuesUpdateInsertParameterizationTests
    {
        [Fact]
        public void SetPropertyValues_UpdateUsesPositionalParameters_InsteadOfNamedInterpolatedValues()
        {
            // Delta: UPDATE switches to positional placeholders to avoid injection.
            const string updateCmd = "UPDATE [aspnet_Profile] SET PropertyNames = ?, PropertyValuesString = ?, PropertyValuesBinary = ?, LastUpdatedDate = ? WHERE UserId = ?";
            const string insertCmd = "INSERT INTO [aspnet_Profile] (UserId, PropertyNames, PropertyValuesString, PropertyValuesBinary, LastUpdatedDate) VALUES (?, ?, ?, ?, ?)";

            Assert.Contains("?", updateCmd);
            Assert.DoesNotContain("$PropertyNames", updateCmd);
            Assert.Contains("VALUES (?, ?, ?, ?, ?)", insertCmd);
        }
    }
}
