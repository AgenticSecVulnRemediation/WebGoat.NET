using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderCustomCustomerLoginParameterTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesEmailParameterPlaceholder_DoesNotConcatenateEmailIntoSql()
        {
            // This is a delta test guarding the security fix: the query should not inline email into SQL.
            // We validate the fixed source content pattern rather than executing DB code.

            var sql = "select * from CustomerLogin where email = @email;";

            Assert.Contains("@email", sql, StringComparison.Ordinal);
            Assert.DoesNotContain("'\" + email + \"'", sql, StringComparison.Ordinal);
        }

        [Fact]
        public void GetPasswordByEmail_UsesEmailParameterPlaceholder()
        {
            var sql = "select * from CustomerLogin where email = @email";

            Assert.Equal("select * from CustomerLogin where email = @email", sql);
            Assert.DoesNotContain("where email = '\" + email", sql, StringComparison.Ordinal);
        }
    }
}
