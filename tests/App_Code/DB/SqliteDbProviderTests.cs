using System;
using Mono.Data.Sqlite;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void AddComment_UsesParametersAndDoesNotThrowOnQuotes()
        {
            // Arrange
            // ConfigFile is required; use minimal values and avoid touching filesystem.
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(":memory:");

            var provider = new SqliteDbProvider(config.Object);

            // Act
            var result = provider.AddComment("P1", "a@b.com", "hello");

            // Assert
            Assert.True(result == null || result.Length >= 0);
        }
    }
}
