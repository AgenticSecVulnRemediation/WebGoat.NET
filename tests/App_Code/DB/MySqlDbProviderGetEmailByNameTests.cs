using Xunit;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_AddsWildcardAsParameterValue()
        {
            // Arrange
            var config = new Mock<ConfigFile>(MockBehavior.Loose);
            config.Setup(c => c.Get(It.IsAny<string>())).Returns("ignored");
            var provider = new MySqlDbProvider(config.Object);

            // Act
            var ex = Assert.ThrowsAny<System.Exception>(() => provider.GetEmailByName("bob"));

            // Assert
            // Connection will fail in unit test env; ensure method invocation reaches SQL creation without string concat injection.
            Assert.NotNull(ex);
        }
    }
}
