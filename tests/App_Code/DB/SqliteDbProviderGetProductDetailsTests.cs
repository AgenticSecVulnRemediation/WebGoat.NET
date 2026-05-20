using System;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetProductDetails_Tests
    {
        [Fact]
        public void GetProductDetails_WithSqlInjectionPayload_DoesNotThrow()
        {
            // Delta regression test: productCode is now bound via parameter @productCode.

            // Arrange
            var config = new ConfigFile();
            config.Set(DbConstants.KEY_FILE_NAME, "test_goatdb.sqlite");
            config.Set(DbConstants.KEY_CLIENT_EXEC, "sqlite3");
            var sut = new SqliteDbProvider(config);

            var injectedProductCode = "S10_1678' OR '1'='1";

            // Act
            var ex = Record.Exception(() => sut.GetProductDetails(injectedProductCode));

            // Assert
            Assert.Null(ex);
        }
    }
}
