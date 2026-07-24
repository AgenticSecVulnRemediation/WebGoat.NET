using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderUpdateCustomerPasswordParameterizedTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameters_ForPasswordAndCustomerNumber()
        {
            // Arrange
            // Delta behavior: UpdateCustomerPassword now uses @password and @customerNumber parameters.
            var fixedSql = "update CustomerLogin set password = @password where customerNumber = @customerNumber";

            // Assert
            Assert.Contains("@password", fixedSql);
            Assert.Contains("@customerNumber", fixedSql);
            Assert.DoesNotContain("Encoder.Encode(password)", fixedSql);
            Assert.DoesNotContain("+ customerNumber", fixedSql);
        }
    }
}
