using System;
using Moq;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_AllowsQuotesInProductCode_DoesNotThrowFromSqlConcatenation()
        {
            // Arrange
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(config.Object);

            // Act
            // A product code containing a quote would previously break the SQL string.
            // With parameterization, the provider should be able to build the command safely.
            var ex = Record.Exception(() => provider.GetProductDetails("ABC'XYZ"));

            // Assert
            Assert.Null(ex);
        }
    }
}
