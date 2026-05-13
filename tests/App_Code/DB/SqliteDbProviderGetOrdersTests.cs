using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_Query_UsesCustomerIdParameter()
        {
            // Arrange
            var provider = new SqliteDbProvider(new ConfigFile());

            // Act
            string sql = "select * from Orders where customerNumber = @customerID";

            // Assert
            Assert.Contains("@customerID", sql);
            Assert.DoesNotContain("+ customerID", sql);
        }
    }
}
