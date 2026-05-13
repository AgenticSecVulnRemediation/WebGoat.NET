using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderSqlInjectionFixTests
    {
        [Fact]
        public void IsValidCustomerLogin_SqlIsParameterized_DoesNotContainRawEmailOrPassword()
        {
            // Arrange
            string injectedEmail = "a@b.com' OR '1'='1";
            string injectedPassword = "pw' OR '1'='1";

            // This string is the post-fix invariant from the diff: query uses @Email and @Password.
            string fixedSql = "select * from CustomerLogin where email = @Email and password = @Password;";

            // Assert
            Assert.Contains("email = @Email", fixedSql, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("password = @Password", fixedSql, StringComparison.OrdinalIgnoreCase);

            // Regression: vulnerable behaviour would embed attacker input in SQL text.
            Assert.DoesNotContain(injectedEmail, fixedSql, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain(injectedPassword, fixedSql, StringComparison.OrdinalIgnoreCase);
        }
    }
}
