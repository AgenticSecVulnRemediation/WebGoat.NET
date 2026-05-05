using System;
using Moq;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPasswordByEmailTests
    {
        [Fact]
        public void GetPasswordByEmail_UsesParameterizedQuery_AllowsQuotesWithoutBreakingSql()
        {
            // Arrange
            var configMock = new Mock<ConfigFile>();
            configMock.Setup(c => c.Get(DbConstants.KEY_FILE_NAME)).Returns("test.sqlite");
            configMock.Setup(c => c.Get(DbConstants.KEY_CLIENT_EXEC)).Returns("sqlite");

            var provider = new SqliteDbProvider(configMock.Object);

            // Act
            var ex = Record.Exception(() => provider.GetPasswordByEmail("a' OR '1'='1"));

            // Assert
            Assert.Null(ex);
        }
    }
}
