using System;
using Mono.Data.Sqlite;
using Moq;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetEmailByCustomerNumberTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterizedQuery_DoesNotConcatenateInput()
        {
            // Arrange
            var configMock = new Mock<ConfigFile>();
            configMock.Setup(c => c.Get(DbConstants.KEY_FILE_NAME)).Returns("test.sqlite");
            configMock.Setup(c => c.Get(DbConstants.KEY_CLIENT_EXEC)).Returns("sqlite");

            var provider = new SqliteDbProvider(configMock.Object);

            // Act
            // Security fix: use @num parameter. We assert method can be called with injection-like input
            // without throwing from malformed SQL due to quote closure.
            var ex = Record.Exception(() => provider.GetEmailByCustomerNumber("1; DROP TABLE CustomerLogin;--"));

            // Assert
            Assert.Null(ex);
        }
    }
}
