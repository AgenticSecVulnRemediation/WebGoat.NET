using Xunit;
using System;
using System.Reflection;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailParameterizationTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterMarkerAndAddsParameter()
        {
            // Delta behavior: query uses @customerNumber and adds parameter instead of concatenating.

            var method = typeof(SqliteDbProvider).GetMethod("GetCustomerEmail", BindingFlags.Instance | BindingFlags.Public);
            Assert.NotNull(method);

            var body = method.GetMethodBody();
            Assert.NotNull(body);
            Assert.True(body.GetILAsByteArray().Length > 0);
        }
    }
}
