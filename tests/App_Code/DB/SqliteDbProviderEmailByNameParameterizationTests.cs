using System;
using Moq;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderEmailByNameParameterizationTests
    {
        [Fact]
        public void GetEmailByName_WithSpecialCharacters_DoesNotThrow()
        {
            // Arrange
            var config = new Mock<ConfigFile>(MockBehavior.Loose);
            config.Setup(c => c.Get(It.IsAny<string>())).Returns("Data Source=:memory:;Version=3");

            var provider = new SqliteDbProvider(config.Object);

            // Act
            // Secure change: SQL uses @Name instead of concatenating name into LIKE clause.
            var ex = Record.Exception(() => provider.GetEmailByName("' OR 1=1 --"));

            // Assert
            Assert.Null(ex);
        }
    }
}
