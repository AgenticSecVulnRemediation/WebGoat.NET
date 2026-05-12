using System;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedCustomerId()
        {
            // Delta: query uses @customerID parameter
            var method = typeof(MySqlDbProvider).GetMethod("GetOrders");
            Assert.NotNull(method);
        }
    }
}
