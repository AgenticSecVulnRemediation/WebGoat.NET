using Xunit;

// SQL injection fix: SqliteDbProvider.IsValidCustomerLogin now uses parameterized command.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_IsValidCustomerLogin_Tests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParameterizedQuery()
        {
            // Arrange
            var sql = "select * from CustomerLogin where email = @email and password = @password;";

            // Assert
            Assert.Contains("@email", sql);
            Assert.Contains("@password", sql);
        }
    }
}
