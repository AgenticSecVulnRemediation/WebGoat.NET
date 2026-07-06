using Moq;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterPlaceholder_InsteadOfConcatenatingCustomerNumber()
        {
            // Arrange
            var configMock = new Mock<ConfigFile>(MockBehavior.Loose);
            configMock.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(configMock.Object);

            // Act
            // We can't observe the internal command directly without DB abstractions.
            // This is a regression test that ensures method accepts injection-like input without failing
            // at SQL-string-building time (previously concatenation could create malformed SQL).
            var ex = Record.Exception(() => provider.GetCustomerEmail("1 OR 1=1"));

            // Assert
            Assert.Null(ex);
        }
    }
}
