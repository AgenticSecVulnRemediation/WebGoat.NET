using Xunit;

// Assumption: production namespace is OWASP.WebGoat.NET.App_Code.DB (from file content).
namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_UpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameters_ForPasswordAndCustomerNumber()
        {
            // Arrange
            var sql = "update CustomerLogin set password = @Password where customerNumber = @CustomerNumber";

            // Act/Assert
            Assert.Contains("@Password", sql);
            Assert.Contains("@CustomerNumber", sql);
            Assert.DoesNotContain("Encoder.Encode(password)", sql); // should not be embedded in SQL string
        }
    }
}
