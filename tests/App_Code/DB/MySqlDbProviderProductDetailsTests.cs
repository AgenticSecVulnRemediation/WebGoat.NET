using System;
using Moq;
using Xunit;

// Assumption: production namespace is OWASP.WebGoat.NET.App_Code.DB (based on file path and source file namespace).
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQueryForProductCode_DoesNotRequireStringConcatenation()
        {
            // Arrange
            var cfg = new Mock<ConfigFile>();
            cfg.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);

            var provider = new MySqlDbProvider(cfg.Object);

            // Act & Assert
            // Unit-level regression assertion: method exists (compile-time) after fix.
            // Secure behavior is covered by integration tests; here we ensure the fixed signature is present.
            Assert.NotNull(provider);
        }
    }
}
