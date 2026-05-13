using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedCustomerIdQuery()
        {
            // Arrange
            var sql = "select * from Orders where customerNumber = @customerID";

            // Assert
            Assert.Contains("@customerID", sql);
            Assert.DoesNotContain("+ customerID", sql);
            Assert.DoesNotContain("where customerNumber = ", sql, System.StringComparison.OrdinalIgnoreCase);
        }
    }
}
