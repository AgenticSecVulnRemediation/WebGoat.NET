using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedCustomerNumber()
        {
            // Arrange
            var method = typeof(MySqlDbProvider).GetMethod("GetOrders");
            Assert.NotNull(method);

            // Act
            const string expectedSql = "select * from Orders where customerNumber = @customerID";

            // Assert
            Assert.Contains("@customerID", expectedSql);
            Assert.DoesNotContain("customerNumber = ", expectedSql.Replace("@customerID", ""), StringComparison.OrdinalIgnoreCase);
        }
    }
}
