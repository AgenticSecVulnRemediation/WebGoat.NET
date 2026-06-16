using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailsParameterizationTests
    {
        [Fact]
        public void GetCustomerEmails_UsesParameterizedLikeQuery()
        {
            // Regression test for SQL injection fix: LIKE should be parameterized using @emailParam.
            var asm = typeof(SqliteDbProvider).Assembly.ToString();

            Assert.Contains("like @emailParam", asm);
            Assert.Contains("@emailParam", asm);
            Assert.DoesNotContain("like '\" + email", asm);
        }
    }
}
