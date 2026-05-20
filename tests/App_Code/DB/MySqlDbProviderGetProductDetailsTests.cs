using System;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_GetProductDetails_Tests
    {
        [Fact]
        public void GetProductDetails_WithSqlInjectionInProductCode_DoesNotThrow()
        {
            // Delta regression test: productCode is now bound via parameter (@productCode)
            // rather than concatenated into SQL.

            // Arrange
            var config = new ConfigFile();
            config.Set(DbConstants.KEY_HOST, "localhost");
            config.Set(DbConstants.KEY_PORT, "3306");
            config.Set(DbConstants.KEY_DATABASE, "goatdb");
            config.Set(DbConstants.KEY_UID, "root");
            config.Set(DbConstants.KEY_PWD, "");
            config.Set(DbConstants.KEY_CLIENT_EXEC, "mysql");

            var sut = new MySqlDbProvider(config);
            var injectedProductCode = "S10_1678' OR '1'='1";

            // Act / Assert
            // We don't assert DB results (requires DB); we assert that the method is robust against payloads
            // that would have altered SQL syntax previously.
            var ex = Record.Exception(() => sut.GetProductDetails(injectedProductCode));
            Assert.Null(ex);
        }
    }
}
