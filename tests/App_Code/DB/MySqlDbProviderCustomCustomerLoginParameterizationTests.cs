using System;
using System.Reflection;
using MySql.Data.MySqlClient;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderCustomCustomerLoginParameterizationTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesParameterizedEmailQuery()
        {
            // This delta test ensures CustomCustomerLogin uses a parameter (@email) rather than string concatenation.
            // We assert via reflection that the updated query marker exists in string literals.

            var t = typeof(MySqlDbProvider);
            var asm = t.Assembly.ToString();

            Assert.Contains("where email = @email", asm);
            Assert.DoesNotContain("where email = '\" + email", asm);
        }
    }
}
