using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_GetProductDetails_Tests
    {
        [Fact]
        public void GetProductDetails_WithQuoteInProductCode_DoesNotThrowFromMalformedSqlConstruction()
        {
            // Arrange
            var provider = new MySqlDbProvider(new ConfigFileStub());

            // Act
            // With parameterization, quotes in productCode should not produce malformed SQL.
            var ex = Record.Exception(() => provider.GetProductDetails("S10_1678' OR '1'='1"));

            // Assert
            Assert.Null(ex);
        }

        private sealed class ConfigFileStub : ConfigFile
        {
            public override string Get(string key)
            {
                if (key == DbConstants.KEY_PWD) return string.Empty;
                if (key == DbConstants.KEY_HOST) return "localhost";
                if (key == DbConstants.KEY_PORT) return "3306";
                if (key == DbConstants.KEY_DATABASE) return "test";
                if (key == DbConstants.KEY_UID) return "root";
                if (key == DbConstants.KEY_CLIENT_EXEC) return "mysql";
                return string.Empty;
            }
        }
    }
}
