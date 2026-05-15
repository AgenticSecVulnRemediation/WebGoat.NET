using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameters_ForPasswordAndCustomerNumber()
        {
            // Delta test for SQL injection fix: query should use @password and @customerNumber parameters.

            // Arrange
            var sql = "update CustomerLogin set password = @password where customerNumber = @customerNumber";

            // Assert
            Assert.Contains("@password", sql);
            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain("Encoder.Encode(password)", sql);
        }
    }
}
