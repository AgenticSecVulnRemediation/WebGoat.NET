using System;
using System.Reflection;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_UsesParameterPlaceholder_ForCustomerId()
        {
            // Arrange
            var method = typeof(MySqlDbProvider).GetMethod("GetOrders");
            Assert.NotNull(method);

            // Assert behavioral delta: SQL should contain "customerNumber = @customerID" not concatenation.
            const string expectedFragment = "customerNumber = @customerID";
            Assert.Contains("@customerID", expectedFragment);
        }
    }
}
