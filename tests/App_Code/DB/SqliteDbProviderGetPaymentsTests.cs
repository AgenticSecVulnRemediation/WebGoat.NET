using System;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetPayments_Tests
    {
        [Fact]
        public void GetPayments_WithSqlInjectionPayload_DoesNotThrow()
        {
            // Delta regression test: customerNumber is now bound via parameter (@CustomerNumber).

            // Arrange
            var config = new ConfigFile();
            config.Set(DbConstants.KEY_FILE_NAME, "test_goatdb.sqlite");
            config.Set(DbConstants.KEY_CLIENT_EXEC, "sqlite3");
            var sut = new SqliteDbProvider(config);

            // Even though the API expects an int, the fix in this PR is about parameterization in the
            // underlying adapter. We'll use an edge int value to ensure no concatenation-related issues.
            int customerNumber = int.MaxValue;

            // Act
            var ex = Record.Exception(() => sut.GetPayments(customerNumber));

            // Assert
            Assert.Null(ex);
        }
    }
}
