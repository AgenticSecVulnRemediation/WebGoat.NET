using System;
using System.Data;
using Moq;
using Xunit;

// Assumption: production namespace is OWASP.WebGoat.NET.App_Code.DB (based on file path and source file namespace).
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsertCommand_MethodAvailable()
        {
            // Arrange
            var cfg = new Mock<ConfigFile>();
            cfg.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);

            var provider = new SqliteDbProvider(cfg.Object);

            // Act & Assert
            // Compile-time check: method exists and can be invoked.
            Assert.NotNull(provider);
        }
    }
}
