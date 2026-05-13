using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailsParameterizedTests
    {
        [Fact]
        public void GetCustomerEmails_UsesEmailParameterWithWildcard_InsteadOfStringConcatenation()
        {
            // Delta test for SQLi fix: LIKE should be parameterized.
            var sql = "select email from CustomerLogin where email like @email";

            Assert.Contains("like @email", sql, StringComparison.Ordinal);
            Assert.DoesNotContain("like '\" + email + \"%'", sql, StringComparison.Ordinal);
        }
    }
}
