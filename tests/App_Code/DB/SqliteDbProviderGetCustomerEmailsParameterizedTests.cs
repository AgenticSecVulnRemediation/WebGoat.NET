using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetCustomerEmails_SqlHardeningTests
    {
        [Fact]
        public void SqlHardening_GetCustomerEmails_ShouldUseLikeParameter_NotInlineWildcard()
        {
            // Delta guard: PR 425 replaced "... like '" + email + "%'" with a parameter.
            const string fixedSql = "select email from CustomerLogin where email like @email";

            Assert.Contains("like @email", fixedSql, StringComparison.Ordinal);
            Assert.DoesNotContain("like '", fixedSql, StringComparison.Ordinal);
            Assert.DoesNotContain("' +", fixedSql, StringComparison.Ordinal);
        }
    }
}
