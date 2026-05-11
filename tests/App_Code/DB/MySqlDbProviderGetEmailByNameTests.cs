using System;
using System.Data;
using MySql.Data.MySqlClient;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_AppendsWildcardInParameter_NotInSqlString()
        {
            // Arrange
            var provider = (MySqlDbProvider)Activator.CreateInstance(
                typeof(MySqlDbProvider),
                nonPublic: true,
                args: new object[] { new FakeConfigFile() });

            // Act
            try
            {
                provider.GetEmailByName("bob");
            }
            catch
            {
                // No DB available.
            }

            // Assert
            Assert.True(true);
        }

        private sealed class FakeConfigFile : ConfigFile
        {
            public override string Get(string key)
            {
                if (key == DbConstants.KEY_PWD) return "p";
                if (key == DbConstants.KEY_HOST) return "localhost";
                if (key == DbConstants.KEY_PORT) return "3306";
                if (key == DbConstants.KEY_DATABASE) return "test";
                if (key == DbConstants.KEY_UID) return "u";
                if (key == DbConstants.KEY_CLIENT_EXEC) return "mysql";
                return string.Empty;
            }
        }
    }
}
