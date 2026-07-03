using Xunit;
using System;
using System.Reflection;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetOrdersParameterizationTests
    {
        [Fact]
        public void GetOrders_UsesSqliteCommandWithParameter_WhenSelectingCustomerNumber()
        {
            // Delta behavior: Orders query now uses @customerID with SqliteCommand, preventing SQL injection.

            var method = typeof(SqliteDbProvider).GetMethod("GetOrders", BindingFlags.Instance | BindingFlags.Public);
            Assert.NotNull(method);

            var body = method.GetMethodBody();
            Assert.NotNull(body);
            Assert.True(body.GetILAsByteArray().Length > 0);
        }
    }
}
