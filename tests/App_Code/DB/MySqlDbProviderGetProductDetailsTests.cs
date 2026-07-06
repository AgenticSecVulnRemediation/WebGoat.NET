using Moq;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_WithSqlInjectionPayload_DoesNotThrowFromSqlConstruction()
        {
            // Arrange
            var configMock = new Mock<ConfigFile>(MockBehavior.Loose);
            configMock.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(configMock.Object);

            // Act
            var ex = Record.Exception(() => provider.GetProductDetails("S10_1678' OR '1'='1"));

            // Assert
            // With parameterization the SQL string should remain valid; any thrown exception here would
            // likely stem from DB access, but string construction should not break.
            Assert.Null(ex);
        }
    }
}
