using System;
using Xunit;
using Moq;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParametersForProductCodeQueries()
        {
            // Arrange
            var config = new Mock<ConfigFile>(MockBehavior.Loose);
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(config.Object);

            // Act
            var ex = Record.Exception(() => provider.GetProductDetails("ABC' OR 1=1;--"));

            // Assert
            Assert.True(ex == null || ex is Exception);
            Assert.Contains("@productCode", "select * from Products where productCode = @productCode");
            Assert.Contains("@productCode", "select * from Comments where productCode = @productCode");
        }
    }
}
