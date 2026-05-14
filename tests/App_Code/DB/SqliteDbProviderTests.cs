using System;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void CustomCustomerLogin_DoesNotThrow_WhenEmailContainsQuotes()
        {
            // Delta test for fix: concatenated SQL -> parameterized SQL in CustomCustomerLogin.

            // Arrange
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new SqliteDbProvider(config.Object);

            string email = "a' OR '1'='1";

            // Act
            var ex = Record.Exception(() => provider.CustomCustomerLogin(email, "any"));

            // Assert
            Assert.Null(ex);
        }

        [Fact]
        public void GetPasswordByEmail_DoesNotThrow_WhenEmailContainsQuotes()
        {
            // Delta test for fix: concatenated SQL -> parameterized SQL in GetPasswordByEmail.

            // Arrange
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new SqliteDbProvider(config.Object);

            string email = "a' OR '1'='1";

            // Act
            var ex = Record.Exception(() => provider.GetPasswordByEmail(email));

            // Assert
            Assert.Null(ex);
        }
    }
}
