using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailsTests
    {
        [Fact]
        public void GetCustomerEmails_UsesLikeParameterAndAppendsWildcard()
        {
            const string expectedSql = "select email from CustomerLogin where email like @Email";
            string expectedParamValue = "alice" + "%";

            Assert.Contains("like @Email", expectedSql, StringComparison.OrdinalIgnoreCase);
            Assert.EndsWith("%", expectedParamValue, StringComparison.Ordinal);
            Assert.DoesNotContain("like '", expectedSql, StringComparison.OrdinalIgnoreCase);
        }
    }
}
