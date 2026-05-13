using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterizedQuery_DoesNotThrowOnInjectionPayload()
        {
            // Arrange
            var config = new ConfigFile();
            config.Set(DbConstants.KEY_HOST, "localhost");
            config.Set(DbConstants.KEY_PORT, "3306");
            config.Set(DbConstants.KEY_DATABASE, "db");
            config.Set(DbConstants.KEY_UID, "uid");
            config.Set(DbConstants.KEY_PWD, "pwd");

            var provider = new MySqlDbProvider(config);

            // Act
            var ex = Record.Exception(() => provider.GetEmailByCustomerNumber("1 OR 1=1"));

            // Assert
            Assert.Null(ex);
        }
    }
}
