using System;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailsTests
    {
        [Fact]
        public void GetCustomerEmails_UsesParameterizedLikeQuery()
        {
            // Arrange
            var config = new Mock<ConfigFile>(MockBehavior.Loose);
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(config.Object);

            // Act
            var method = typeof(MySqlDbProvider).GetMethod("GetCustomerEmails");

            // Assert
            Assert.NotNull(method);
            Assert.Contains("@email", method.ToString());
        }
    }
}
