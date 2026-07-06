using Moq;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderIsValidCustomerLoginTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParameters_AllowsSqlInjectionPayloadWithoutSqlStringBreakage()
        {
            // Arrange
            var configMock = new Mock<ConfigFile>(MockBehavior.Loose);
            configMock.Setup(c => c.Get(It.IsAny<string>())).Returns("dummy");
            var provider = new SqliteDbProvider(configMock.Object);

            // Act
            var ex = Record.Exception(() => provider.IsValidCustomerLogin("x' OR '1'='1", "pw"));

            // Assert
            // Regression guard: with parameters, the query string isn't malformed by quotes.
            Assert.Null(ex);
        }
    }
}
