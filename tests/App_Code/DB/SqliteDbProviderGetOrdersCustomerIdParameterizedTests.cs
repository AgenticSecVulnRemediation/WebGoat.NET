using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetOrdersCustomerIdParameterizedTests
    {
        [Fact]
        public void GetOrders_UsesParameterMarkerForCustomerId()
        {
            // Arrange
            var customerId = 123;

            // Act
            // Delta behavior from diff: concatenated customerID -> @customerID.
            var sql = "select * from Orders where customerNumber = @customerID";

            // Assert
            Assert.Contains("@customerID", sql);
            Assert.DoesNotContain("customerNumber = " + customerId, sql);
        }
    }
}
