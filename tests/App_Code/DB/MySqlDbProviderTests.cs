using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void AddComment_DoesNotConcatenateInputIntoSql_DoesNotThrowOnSqlInjectionPayload()
        {
            // Arrange
            var config = new ConfigFile();
            config.Set(DbConstants.KEY_HOST, "localhost");
            config.Set(DbConstants.KEY_PORT, "3306");
            config.Set(DbConstants.KEY_DATABASE, "db");
            config.Set(DbConstants.KEY_UID, "uid");
            config.Set(DbConstants.KEY_PWD, "pwd");

            var provider = new MySqlDbProvider(config);
            string productCode = "P1";
            string email = "x@example.com";
            string comment = "test'); DROP TABLE Comments; --";

            // Act
            var ex = Record.Exception(() => provider.AddComment(productCode, email, comment));

            // Assert
            Assert.Null(ex);
        }
    }
}
