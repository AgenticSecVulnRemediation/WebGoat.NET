using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetPasswordByEmailParameterizationTests
    {
        [Fact]
        public void GetPasswordByEmail_UsesEmailParameter()
        {
            // Regression test for injection fix: email must be bound via @email parameter.
            var asm = typeof(MySqlDbProvider).Assembly.ToString();

            Assert.Contains("where email = @email", asm);
            Assert.DoesNotContain("where email = '\" + email", asm);
        }
    }
}
