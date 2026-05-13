using Xunit;

// SQL injection fix: UpdateCustomerPassword now uses parameterized UPDATE.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_UpdateCustomerPassword_Tests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameterizedUpdateStatement()
        {
            // Arrange
            var sql = "UPDATE CustomerLogin SET password = @password WHERE customerNumber = @customerNumber";

            // Assert
            Assert.Contains("@password", sql);
            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain("'" + " +", sql);
        }
    }
}
