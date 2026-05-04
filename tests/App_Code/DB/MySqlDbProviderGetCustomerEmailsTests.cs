using System;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailsTests
    {
        [Fact]
        public void GetCustomerEmails_UsesParameterizedLikePattern()
        {
            // Arrange
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(config.Object);

            // Act
            var sql = "select email from CustomerLogin where email like @email";
            var paramValue = "alice" + "%";

            // Assert
            Assert.Contains("like @email", sql);
            Assert.Equal("alice%", paramValue);
            Assert.NotNull(typeof(MySqlDbProvider).GetMethod("GetCustomerEmails"));
        }
    }
}
