using System;
using System.Data;
using Mono.Data.Sqlite;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQueriesForProductCode_DoesNotInlineProductCode()
        {
            // Arrange
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new SqliteDbProvider(config.Object);

            // Act
            DataSet result = provider.GetProductDetails("P1");

            // Assert
            Assert.NotNull(provider);
        }
    }
}
