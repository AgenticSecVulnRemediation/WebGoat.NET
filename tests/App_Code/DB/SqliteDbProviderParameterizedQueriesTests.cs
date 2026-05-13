using System;
using Moq;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderParameterizedQueriesTests
    {
        [Fact]
        public void CustomCustomerLogin_WithSqlInjectionLikeEmail_DoesNotThrowArgumentNullException()
        {
            // Delta focus from diff: CustomCustomerLogin now uses parameterized query ("email = @email").
            var cfg = new Mock<ConfigFile>();
            cfg.Setup(c => c.Get(It.IsAny<string>())).Returns("test.db");
            var provider = new SqliteDbProvider(cfg.Object);

            var ex = Record.Exception(() => provider.CustomCustomerLogin("x' OR '1'='1", "pw"));
            Assert.False(ex is ArgumentNullException);
        }

        [Fact]
        public void GetPasswordByEmail_WithSqlInjectionLikeEmail_DoesNotThrowArgumentNullException()
        {
            // Delta focus from diff: GetPasswordByEmail now uses parameterized query ("email = @email").
            var cfg = new Mock<ConfigFile>();
            cfg.Setup(c => c.Get(It.IsAny<string>())).Returns("test.db");
            var provider = new SqliteDbProvider(cfg.Object);

            var ex = Record.Exception(() => provider.GetPasswordByEmail("x' OR '1'='1"));
            Assert.False(ex is ArgumentNullException);
        }
    }
}
