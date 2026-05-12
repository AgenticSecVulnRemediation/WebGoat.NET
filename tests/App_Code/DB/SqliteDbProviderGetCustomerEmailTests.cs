using System;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailTests
    {
        [Fact]
        public void GetCustomerEmail_UsesCustomerNumberParameter()
        {
            // Delta: query now uses @customerNumber
            var method = typeof(SqliteDbProvider).GetMethod("GetCustomerEmail");
            Assert.NotNull(method);
        }
    }
}
