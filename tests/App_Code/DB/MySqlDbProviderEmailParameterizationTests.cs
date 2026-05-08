using System;
using System.Text;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderEmailParameterizationTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesEmailParameter()
        {
            // Delta regression test: email lookup changed to @email parameter.
            var asmText = Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(typeof(MySqlDbProvider).Assembly.Location));
            Assert.Contains("where email = @email", asmText);
        }

        [Fact]
        public void GetPasswordByEmail_UsesEmailParameter()
        {
            // Delta regression test: email lookup changed to @email parameter.
            var asmText = Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(typeof(MySqlDbProvider).Assembly.Location));
            Assert.Contains("select * from CustomerLogin where email = @email", asmText);
        }
    }
}
