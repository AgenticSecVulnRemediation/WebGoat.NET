using System;
using System.Data;
using MySql.Data.MySqlClient;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_WithValue_ReturnsDataSetOrNull_WithoutStringConcatenationFailures()
        {
            // Arrange
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(config.Object);

            // Act
            DataSet ds = provider.GetProductDetails("S10_1678");

            // Assert
            Assert.NotNull(ds);
        }
    }
}
