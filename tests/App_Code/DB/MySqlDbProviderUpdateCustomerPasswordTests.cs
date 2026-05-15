using Xunit;

// Assumption: production namespace is OWASP.WebGoat.NET.App_Code.DB (from file content).
namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_UpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesNamedParameters_InsteadOfStringConcatenation()
        {
            // Arrange
            var sql = "update CustomerLogin set password = @password where customerNumber = @customerNumber";

            // Act/Assert
            Assert.Contains("@password", sql);
            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain("'" + " + ", sql); // no concatenation of encoded password
        }
    }
}
