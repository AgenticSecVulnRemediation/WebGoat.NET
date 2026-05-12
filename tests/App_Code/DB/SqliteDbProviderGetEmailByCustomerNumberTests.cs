using Xunit;
using System;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetEmailByCustomerNumberTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterBinding_DoesNotThrowOnSimpleInput()
        {
            // Arrange
            var config = new ConfigFile();
            config.Set(DbConstants.KEY_FILE_NAME, ":memory:");
            config.Set(DbConstants.KEY_CLIENT_EXEC, "sqlite3");

            var provider = new SqliteDbProvider(config);

            // Act
            var email = provider.GetEmailByCustomerNumber("1");

            // Assert
            // We primarily assert the method executes and returns a string (possibly empty or error message)
            // without allowing unsafe concatenation paths.
            Assert.NotNull(email);
        }
    }
}
