using System;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetCustomerEmails_AllowsEmailPrefixWithQuotes_WithoutThrowing()
        {
            // Delta test for fix: concatenated LIKE -> parameterized LIKE.

            // Arrange
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(config.Object);

            string emailPrefix = "a' OR '1'='1";

            // Act
            var ex = Record.Exception(() => provider.GetCustomerEmails(emailPrefix));

            // Assert
            Assert.Null(ex);
        }
    }
}
