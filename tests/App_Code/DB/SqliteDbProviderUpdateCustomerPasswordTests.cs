using System;
using System.Text;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParametersInsteadOfStringConcatenation()
        {
            // Delta regression test: SQL changed to use @password and @customerNumber parameters.
            var asmText = Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(typeof(SqliteDbProvider).Assembly.Location));

            Assert.Contains("SET password = @password", asmText);
            Assert.Contains("customerNumber = @customerNumber", asmText);
        }
    }
}
