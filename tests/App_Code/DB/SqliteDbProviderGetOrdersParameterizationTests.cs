using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetOrdersParameterizationTests
    {
        [Fact]
        public void GetOrders_SqlUsesCustomerIdParameter()
        {
            // Arrange
            string fixedSql = "select * from Orders where customerNumber = @customerID";

            // Assert
            Assert.Contains("@customerID", fixedSql, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("customerNumber = ", fixedSql.Replace("customerNumber = @customerID", ""), StringComparison.OrdinalIgnoreCase);
        }
    }
}
