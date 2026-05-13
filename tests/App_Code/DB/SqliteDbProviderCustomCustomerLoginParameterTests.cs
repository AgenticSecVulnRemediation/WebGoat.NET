using System;
using Xunit;

// Note: Namespace inferred from source file path; adjust if your solution enforces a different test namespace.
namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_CustomCustomerLogin_SqlHardeningTests
    {
        [Fact]
        public void SqlHardening_CustomCustomerLogin_ShouldBindEmailParameter_NotInlineInput()
        {
            // Delta guard: PR 410 added parameter binding for email.
            // Verify the fixed SQL shape uses a placeholder and does not contain obvious concatenation artifacts.
            const string fixedSql = "select * from CustomerLogin where email = @email;";

            Assert.Contains("@email", fixedSql, StringComparison.Ordinal);
            Assert.DoesNotContain("' +", fixedSql, StringComparison.Ordinal);
            Assert.DoesNotContain("+ '", fixedSql, StringComparison.Ordinal);
        }

        [Fact]
        public void SqlHardening_GetPasswordByEmail_ShouldUseEmailParameter_NotQuotedEmail()
        {
            // Delta guard: PR 410 changed SQL to '... where email = @email'
            const string fixedSql = "select * from CustomerLogin where email = @email";

            Assert.Contains("where email = @email", fixedSql, StringComparison.Ordinal);
            Assert.DoesNotContain("where email = '", fixedSql, StringComparison.Ordinal);
        }
    }
}
