using System;
using System.Text;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderIsValidCustomerLoginTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParametersForEmailAndPassword()
        {
            // Delta regression test: SQL changed to parameterized query with @Email and @Password.
            var asmBytes = System.IO.File.ReadAllBytes(typeof(MySqlDbProvider).Assembly.Location);
            var asmText = Encoding.UTF8.GetString(asmBytes);

            Assert.Contains("where email = @Email", asmText);
            Assert.Contains("password = @Password", asmText);
        }
    }
}
