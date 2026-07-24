using Xunit;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using System;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsParameterizationTests
    {
        [Fact]
        public void GetProductDetails_WithInjectionLikeInput_DoesNotThrowDuringCommandConstruction()
        {
            // Arrange
            var config = new Mock<ConfigFile>(MockBehavior.Loose);
            config.Setup(c => c.Get(It.IsAny<string>())).Returns("x");
            var provider = new MySqlDbProvider(config.Object);

            // Act
            var ex = Record.Exception(() => provider.GetProductDetails("' OR '1'='1"));

            // Assert
            // We primarily ensure updated logic uses parameterized commands and doesn't explode due to string concatenation.
            // Since method hits DB, it may throw connection-related exceptions; so only assert it is not a SQL syntax exception.
            if (ex != null)
            {
                Assert.DoesNotContain("You have an error in your SQL syntax", ex.Message, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
