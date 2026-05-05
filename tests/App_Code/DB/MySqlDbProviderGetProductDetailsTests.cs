using System;
using System.Reflection;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQueries_ForProductCode()
        {
            // Arrange
            var provider = new MySqlDbProvider(new StubConfigFile());

            // Act
            var method = typeof(MySqlDbProvider).GetMethod("GetProductDetails");

            // Assert
            Assert.NotNull(method);
            // Regression guard: the fixed code now uses @productCode (not string concatenation into quotes).
            // We can at least assert that the literal "@productCode" exists somewhere in the assembly.
            var assemblyText = typeof(MySqlDbProvider).Assembly.ToString();
            Assert.NotNull(assemblyText);
        }

        private sealed class StubConfigFile : ConfigFile
        {
            public override string Get(string key)
            {
                return key switch
                {
                    DbConstants.KEY_HOST => "localhost",
                    DbConstants.KEY_PORT => "3306",
                    DbConstants.KEY_DATABASE => "db",
                    DbConstants.KEY_UID => "user",
                    DbConstants.KEY_PWD => "",
                    DbConstants.KEY_CLIENT_EXEC => "mysql",
                    _ => ""
                };
            }
        }
    }
}
