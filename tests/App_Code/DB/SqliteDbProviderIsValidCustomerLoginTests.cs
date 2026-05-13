using System;
using Moq;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderIsValidCustomerLoginTests
    {
        [Fact]
        public void IsValidCustomerLogin_WithSqlInjectionLikeEmail_DoesNotThrowArgumentNullException()
        {
            // Delta focus: query now parameterized with @email and @password.
            var cfg = new Mock<ConfigFile>();
            cfg.Setup(c => c.Get(It.IsAny<string>())).Returns("test.db");

            var provider = new SqliteDbProvider(cfg.Object);

            var ex = Record.Exception(() => provider.IsValidCustomerLogin("x' OR '1'='1", "pw"));
            Assert.False(ex is ArgumentNullException);
        }

        [Fact]
        public void IsValidCustomerLogin_WhenNoRows_ReturnsFalse()
        {
            // Delta focus: ensure semantic expectation: no matching user should not authenticate.
            // NOTE: Current implementation in new_file_content returns ds.Tables[0].Rows.Count == 0 (buggy, would return true).
            // This test will catch that regression.

            var cfg = new Mock<ConfigFile>();
            cfg.Setup(c => c.Get(It.IsAny<string>())).Returns("test.db");
            var provider = new SqliteDbProvider(cfg.Object);

            bool result;
            try
            {
                result = provider.IsValidCustomerLogin("doesnotexist@example.com", "pw");
            }
            catch
            {
                // Without DB setup, we can't assert result; but we still want to enforce the fixed behavior contract.
                // Re-throw to fail in environments where DB is available.
                throw;
            }

            Assert.False(result);
        }
    }
}
