using System;
using System.Text;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterForCustomerNumber()
        {
            // Delta regression test: query changed from concatenation to parameter @customerNumber.
            var asmBytes = System.IO.File.ReadAllBytes(typeof(MySqlDbProvider).Assembly.Location);
            var asmText = Encoding.UTF8.GetString(asmBytes);

            Assert.Contains("customerNumber = @customerNumber", asmText);
        }
    }
}
