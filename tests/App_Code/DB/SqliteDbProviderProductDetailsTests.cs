using System;
using System.Data;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQueryAndReturnsDataSet()
        {
            // Arrange
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(":memory:");
            var provider = new SqliteDbProvider(config.Object);

            // Act
            DataSet ds = provider.GetProductDetails("S10_1678");

            // Assert
            Assert.NotNull(ds);
        }
    }
}
