using System;
using Moq;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByNameTests
    {
        [Theory]
        [InlineData("Alice")]
        [InlineData("A%")]
        public void GetEmailByName_AcceptsInput_DoesNotThrow(string name)
        {
            // Arrange
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(config.Object);

            // Act
            var ex = Record.Exception(() => provider.GetEmailByName(name));

            // Assert
            Assert.Null(ex);
        }
    }
}
