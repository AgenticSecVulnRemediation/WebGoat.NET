using System;
using Moq;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailsTests
    {
        [Fact]
        public void GetCustomerEmails_WithSqlWildcardAndQuotes_DoesNotThrowArgumentNullException()
        {
            // Delta focus: LIKE clause now parameterized using CONCAT(@email, '%').
            var cfg = new Mock<ConfigFile>();
            cfg.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(cfg.Object);

            var ex = Record.Exception(() => provider.GetCustomerEmails("a%' OR '1'='1"));
            Assert.False(ex is ArgumentNullException);
        }
    }
}
