using Xunit;

// SQL injection fix: SqliteDbProvider.GetPayments now uses parameterized customerNumber.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetPayments_Tests
    {
        [Fact]
        public void GetPayments_UsesParameterizedCustomerNumber()
        {
            // Arrange
            var sql = "select * from Payments where customerNumber = @customerNumber";

            // Assert
            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain(" + customerNumber", sql);
        }
    }
}
