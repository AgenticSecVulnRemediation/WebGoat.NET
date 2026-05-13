using Xunit;

namespace OWASP.WebGoat.NET.Tests.Code
{
    public class SQLiteProfileProviderQueryTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_UsesAtUserIdParameter()
        {
            // Arrange
            const string sql = "SELECT PropertyNames, PropertyValuesString, PropertyValuesBinary FROM [aspnet_Profile] WHERE UserId = @UserId";

            // Assert
            Assert.Contains("@UserId", sql);
            Assert.DoesNotContain("$UserId", sql);
        }
    }
}
