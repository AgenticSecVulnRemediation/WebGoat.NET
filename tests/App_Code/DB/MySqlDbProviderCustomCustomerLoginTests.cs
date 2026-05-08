using System;
using System.Text;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderCustomCustomerLoginTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesEmailParameter()
        {
            // Delta regression test: email lookup changed to use @Email parameter.
            var asmText = Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(typeof(MySqlDbProvider).Assembly.Location));

            Assert.Contains("where email = @Email", asmText);
            Assert.Contains("Parameters.AddWithValue(\"@Email\"", asmText);
        }
    }
}
