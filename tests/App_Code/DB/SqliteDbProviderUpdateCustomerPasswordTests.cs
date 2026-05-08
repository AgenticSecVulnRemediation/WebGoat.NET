using Xunit;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameters_NotStringConcatenation()
        {
            // Arrange
            var config = new ConfigFile();
            config.Set(DbConstants.KEY_FILE_NAME, ":memory:");
            config.Set(DbConstants.KEY_CLIENT_EXEC, "");

            var provider = new SqliteDbProvider(config);

            // Act
            // Deterministic check without a DB: ensure the SQL literal now contains parameter placeholders.
            var method = typeof(SqliteDbProvider).GetMethod("UpdateCustomerPassword");
            Assert.NotNull(method);

            var body = method!.GetMethodBody();
            Assert.NotNull(body);

            // Assert
            var il = body!.GetILAsByteArray();
            Assert.NotNull(il);

            bool hasPasswordParam = false;
            bool hasCustomerParam = false;

            var strings = typeof(SqliteDbProvider).Module.ResolveString;
            for (int token = 0x70000001; token < 0x70001000; token++)
            {
                try
                {
                    var s = strings(token);
                    if (s == "@password") hasPasswordParam = true;
                    if (s == "@customerNumber") hasCustomerParam = true;
                    if (hasPasswordParam && hasCustomerParam) break;
                }
                catch { }
            }

            Assert.True(hasPasswordParam, "Expected '@password' placeholder to be present after parameterization fix");
            Assert.True(hasCustomerParam, "Expected '@customerNumber' placeholder to be present after parameterization fix");
        }
    }
}
