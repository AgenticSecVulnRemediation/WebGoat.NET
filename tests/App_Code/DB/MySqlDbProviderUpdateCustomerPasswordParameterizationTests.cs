using System;
using System.Text;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderUpdateCustomerPasswordParameterizationTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameters()
        {
            // Delta regression test: update now uses @password and @customerNumber parameters.
            var asmText = Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(typeof(MySqlDbProvider).Assembly.Location));

            Assert.Contains("set password = @password", asmText);
            Assert.Contains("customerNumber = @customerNumber", asmText);
        }
    }
}
