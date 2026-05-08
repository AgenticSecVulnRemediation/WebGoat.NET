using System;
using System.Text;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetOrdersParameterizationTests
    {
        [Fact]
        public void GetOrders_UsesParameterForCustomerId()
        {
            // Delta regression test: customerNumber filter changed to @customerID parameter.
            var asmText = Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(typeof(SqliteDbProvider).Assembly.Location));
            Assert.Contains("customerNumber = @customerID", asmText);
        }
    }
}
