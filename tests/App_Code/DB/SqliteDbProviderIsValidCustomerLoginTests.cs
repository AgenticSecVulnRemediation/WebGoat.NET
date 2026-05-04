using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_IsValidCustomerLogin_Tests
    {
        [Fact]
        public void IsValidCustomerLogin_WithQuoteInEmail_DoesNotThrowFromMalformedSqlConstruction()
        {
            // Arrange
            var provider = new SqliteDbProvider(new ConfigFileStub());

            // Act
            // With parameterized SQL, quote in email should not break the query string.
            var ex = Record.Exception(() => provider.IsValidCustomerLogin("a' OR '1'='1", "pw"));

            // Assert
            Assert.Null(ex);
        }

        private sealed class ConfigFileStub : ConfigFile
        {
            public override string Get(string key)
            {
                if (key == DbConstants.KEY_FILE_NAME) return "test.sqlite";
                if (key == DbConstants.KEY_CLIENT_EXEC) return "sqlite3";
                return string.Empty;
            }
        }
    }
}
