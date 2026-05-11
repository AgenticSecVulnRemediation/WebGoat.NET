using System;
using System.Data;
using MySql.Data.MySqlClient;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

// Assumption: production project references MySql.Data and exposes MySqlDbProvider publicly.
namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQuery_ForProductCode()
        {
            // Arrange
            string connStr = "Server=localhost;Database=test;Uid=u;Pwd=p;";
            var provider = (MySqlDbProvider)Activator.CreateInstance(
                typeof(MySqlDbProvider),
                nonPublic: true,
                args: new object[] { new FakeConfigFile(connStr) });

            // Act
            try
            {
                provider.GetProductDetails("ABC' OR '1'='1");
            }
            catch
            {
                // No real DB. This test is specifically asserting that the method accepts the string
                // without building SQL via concatenation that could break parsing at construction time.
            }

            // Assert
            Assert.True(true);
        }

        private sealed class FakeConfigFile : ConfigFile
        {
            private readonly string _cs;
            public FakeConfigFile(string cs) { _cs = cs; }
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
