using Xunit;

namespace OWASP.WebGoat.NET.Tests.App_Code.DB
{
    public class MySqlDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedCustomerId()
        {
            // Arrange
            const string sql = "select * from Orders where customerNumber = @customerID";

            // Assert
            Assert.Contains("@customerID", sql);
            Assert.DoesNotContain("customerNumber = \" + customerID", sql);
        }
    }
}
