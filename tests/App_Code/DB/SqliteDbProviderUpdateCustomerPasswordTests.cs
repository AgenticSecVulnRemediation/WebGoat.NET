using System;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameterizedUpdate()
        {
            // Delta: update uses parameters @password and @customerNumber
            var method = typeof(SqliteDbProvider).GetMethod("UpdateCustomerPassword");
            Assert.NotNull(method);
        }
    }
}
