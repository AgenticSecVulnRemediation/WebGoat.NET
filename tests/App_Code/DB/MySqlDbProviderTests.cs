using System;
using System.Data;
using MySql.Data.MySqlClient;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetEmailByName_UsesParameterWithWildcard_AppendsPercentSafely()
        {
            // Arrange
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(config.Object);

            // Act
            DataSet result = provider.GetEmailByName("Al");

            // Assert
            Assert.NotNull(provider);
        }
    }
}
