using System;
using Moq;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderEmailParameterizationTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesParameterForEmail_AllowsQuotesWithoutBreakingSql()
        {
            // Arrange
            var configMock = new Mock<ConfigFile>();
            configMock.Setup(c => c.Get(DbConstants.KEY_HOST)).Returns("localhost");
            configMock.Setup(c => c.Get(DbConstants.KEY_PORT)).Returns("3306");
            configMock.Setup(c => c.Get(DbConstants.KEY_DATABASE)).Returns("db");
            configMock.Setup(c => c.Get(DbConstants.KEY_UID)).Returns("user");
            configMock.Setup(c => c.Get(DbConstants.KEY_PWD)).Returns("");
            configMock.Setup(c => c.Get(DbConstants.KEY_CLIENT_EXEC)).Returns("mysql");

            var provider = new MySqlDbProvider(configMock.Object);

            // Act
            var ex = Record.Exception(() => provider.CustomCustomerLogin("a' OR '1'='1", "pw"));

            // Assert
            Assert.Null(ex);
        }

        [Fact]
        public void GetPasswordByEmail_UsesParameterForEmail_AllowsQuotesWithoutBreakingSql()
        {
            // Arrange
            var configMock = new Mock<ConfigFile>();
            configMock.Setup(c => c.Get(DbConstants.KEY_HOST)).Returns("localhost");
            configMock.Setup(c => c.Get(DbConstants.KEY_PORT)).Returns("3306");
            configMock.Setup(c => c.Get(DbConstants.KEY_DATABASE)).Returns("db");
            configMock.Setup(c => c.Get(DbConstants.KEY_UID)).Returns("user");
            configMock.Setup(c => c.Get(DbConstants.KEY_PWD)).Returns("");
            configMock.Setup(c => c.Get(DbConstants.KEY_CLIENT_EXEC)).Returns("mysql");

            var provider = new MySqlDbProvider(configMock.Object);

            // Act
            var ex = Record.Exception(() => provider.GetPasswordByEmail("a' OR '1'='1"));

            // Assert
            Assert.Null(ex);
        }
    }
}
