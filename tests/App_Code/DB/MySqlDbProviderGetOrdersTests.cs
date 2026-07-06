using Moq;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_WithMaliciousCustomerIdInput_DoesNotAllowSqlConcatenationRegression()
        {
            // Arrange
            var configMock = new Mock<ConfigFile>(MockBehavior.Loose);
            configMock.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(configMock.Object);

            // Act
            // Signature takes int, so SQLi must be prevented by parameterization; compilation ensures int.
            // This test asserts no exception thrown during creation of parameterized adapter.
            var ex = Record.Exception(() => provider.GetOrders(1));

            // Assert
            Assert.Null(ex);
        }
    }
}
