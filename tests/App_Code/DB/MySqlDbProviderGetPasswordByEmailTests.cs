using System;
using Moq;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetPasswordByEmailTests
    {
        [Fact]
        public void GetPasswordByEmail_WithSqlLikeEmailInput_DoesNotThrowArgumentNullException()
        {
            // Delta focus: query changed to use @email parameter.
            var cfg = new Mock<ConfigFile>();
            cfg.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(cfg.Object);

            var ex = Record.Exception(() => provider.GetPasswordByEmail("x' OR '1'='1"));
            Assert.False(ex is ArgumentNullException);
        }
    }
}
