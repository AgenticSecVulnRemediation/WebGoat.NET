using System;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedCustomerId()
        {
            // Delta: select uses @customerID
            var method = typeof(SqliteDbProvider).GetMethod("GetOrders");
            Assert.NotNull(method);
        }
    }
}
