using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesParameterizedQuery_DoesNotThrowOnInjectionPayload()
        {
            // Arrange
            var config = new ConfigFile();
            config.Set(DbConstants.KEY_FILE_NAME, ":memory:");
            config.Set(DbConstants.KEY_CLIENT_EXEC, "sqlite3");

            var provider = new SqliteDbProvider(config);

            // Act
            var ex = Record.Exception(() => provider.CustomCustomerLogin("x' OR 1=1 --", "pw"));

            // Assert
            Assert.Null(ex);
        }
    }
}
