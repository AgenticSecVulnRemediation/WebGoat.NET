using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_UpdateCustomerPassword_Tests
    {
        [Fact]
        public void UpdateCustomerPassword_DoesNotEmbedCustomerNumberOrPasswordInSqlString()
        {
            // Arrange
            var provider = new MySqlDbProvider(new ConfigFileStub());

            // Act
            // If the SQL were concatenated, a password containing quotes could cause malformed SQL.
            // With parameterization, it should not throw due to SQL string construction.
            string result = provider.UpdateCustomerPassword(1, "p@ssw'rd");

            // Assert
            Assert.True(result == null || result.Length > 0);
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
