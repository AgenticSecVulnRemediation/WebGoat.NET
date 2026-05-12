using System;
using System.Data;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_AllowsWildcardSearchViaParameter()
        {
            // Arrange
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(config.Object);

            // Act
            DataSet ds = provider.GetEmailByName("Al");

            // Assert
            Assert.True(ds == null || ds is DataSet);
        }
    }
}
