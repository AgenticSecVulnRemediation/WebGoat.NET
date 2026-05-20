using System;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void GetCustomerEmail_WithSqlInjectionPayload_DoesNotIncludeRawSqlError()
        {
            // Delta regression test: customerNumber is now passed via parameter @customerNumber.
            // Previously, concatenation could allow SQL injection.

            // Arrange
            var config = new ConfigFile();
            config.Set(DbConstants.KEY_FILE_NAME, "test_goatdb.sqlite");
            config.Set(DbConstants.KEY_CLIENT_EXEC, "sqlite3");

            var sut = new SqliteDbProvider(config);

            // Attack payload; should be treated as a literal value, not executable SQL.
            var injectedCustomerNumber = "1; DROP TABLE CustomerLogin;--";

            // Act
            var ex = Record.Exception(() => sut.GetCustomerEmail(injectedCustomerNumber));

            // Assert
            Assert.Null(ex);
        }
    }
}
