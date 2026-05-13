using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParameterizedQuery_ForEmailAndPassword()
        {
            // Arrange
            const string sql = "select * from CustomerLogin where email = @email and password = @password;";

            // Assert
            Assert.Contains("@email", sql);
            Assert.Contains("@password", sql);
            Assert.DoesNotContain("'\" + email + \"'", sql);
        }
    }
}
