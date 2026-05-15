using Xunit;

// Assumption: production namespace is OWASP.WebGoat.NET.App_Code.DB (from file content).
namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetPaymentsTests
    {
        [Fact]
        public void GetPayments_UsesCustomerNumberParameter_InsteadOfConcatenation()
        {
            // Arrange
            var sql = "select * from Payments where customerNumber = @customerNumber";

            // Act/Assert
            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain("+ customerNumber", sql);
        }
    }
}
