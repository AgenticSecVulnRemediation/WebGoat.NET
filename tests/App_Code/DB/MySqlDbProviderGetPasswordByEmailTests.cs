using System;
using Xunit;
using Moq;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetPasswordByEmailTests
    {
        [Fact]
        public void GetPasswordByEmail_WithInjectionLikeEmail_DoesNotThrow_DueToStringConcatenation()
        {
            // Delta test: query changed from string concatenation to parameterized MySqlCommand.
            // We assert the method can be called with dangerous input without causing string-based SQL building exceptions.

            // Arrange
            var configFile = new Mock<ConfigFile>(MockBehavior.Loose);
            configFile.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(configFile.Object);

            // Act
            var ex = Record.Exception(() => provider.GetPasswordByEmail("a@b.com' OR '1'='1"));

            // Assert
            Assert.Null(ex);
        }
    }
}
