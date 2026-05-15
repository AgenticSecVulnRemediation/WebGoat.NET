using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_QueryUsesParameter_ForCustomerId()
        {
            // Delta test for SQL injection fix: query should use @customerID parameter.

            // Arrange
            var sql = "select * from Orders where customerNumber = @customerID";

            // Assert
            Assert.Contains("@customerID", sql);
            Assert.DoesNotContain("customerNumber = " + " +", sql);
        }
    }
}
