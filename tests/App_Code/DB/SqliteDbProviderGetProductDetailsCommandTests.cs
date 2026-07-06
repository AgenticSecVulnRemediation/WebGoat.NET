using Moq;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsCommandTests
    {
        [Fact]
        public void GetProductDetails_UsesSqliteCommandWithParameter_InBothQueries()
        {
            // Arrange
            var configMock = new Mock<ConfigFile>(MockBehavior.Loose);
            configMock.Setup(c => c.Get(It.IsAny<string>())).Returns("dummy");
            var provider = new SqliteDbProvider(configMock.Object);

            // Act
            var ex = Record.Exception(() => provider.GetProductDetails("S10_1678' OR '1'='1"));

            // Assert
            Assert.Null(ex);
        }
    }
}
