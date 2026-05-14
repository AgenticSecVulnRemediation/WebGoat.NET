using System;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetProductDetails_AllowsProductCodeWithQuotes_WithoutThrowing_FromSqlConcatenation()
        {
            // Delta test for fix: SQL concatenation -> parameterized selects.

            // Arrange
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(config.Object);

            string productCode = "S10_1678' OR '1'='1";

            // Act
            var ex = Record.Exception(() => provider.GetProductDetails(productCode));

            // Assert
            Assert.Null(ex);
        }
    }
}
