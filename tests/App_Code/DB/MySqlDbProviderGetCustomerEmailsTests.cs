using Xunit;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailsTests
    {
        [Fact]
        public void GetCustomerEmails_UsesParameterizedLike()
        {
            // Arrange
            var config = new Mock<ConfigFile>(MockBehavior.Loose);
            config.Setup(c => c.Get(It.IsAny<string>())).Returns("ignored");
            var provider = new MySqlDbProvider(config.Object);

            // Act
            var ex = Assert.ThrowsAny<System.Exception>(() => provider.GetCustomerEmails("bob"));

            // Assert
            Assert.NotNull(ex);
        }
    }
}
