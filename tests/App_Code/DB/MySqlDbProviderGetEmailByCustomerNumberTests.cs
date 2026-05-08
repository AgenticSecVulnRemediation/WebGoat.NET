using System;
using System.Text;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByCustomerNumberTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterizedExecuteScalar()
        {
            // Delta regression test: moved from string concatenation to ExecuteScalar with @CustomerNumber parameter.
            var asmText = Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(typeof(MySqlDbProvider).Assembly.Location));

            Assert.Contains("customerNumber = @CustomerNumber", asmText);
            Assert.Contains("new MySqlParameter(\"@CustomerNumber\"", asmText);
        }
    }
}
