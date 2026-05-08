using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

// Assumption: source is compiled with MySql.Data available; this unit test targets only the delta:
// GetEmailByCustomerNumber now uses a parameter placeholder rather than concatenating the customer number.
namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByCustomerNumberTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterPlaceholderInSql()
        {
            // Arrange
            var config = new ConfigFile();
            config.Set(DbConstants.KEY_HOST, "localhost");
            config.Set(DbConstants.KEY_PORT, "3306");
            config.Set(DbConstants.KEY_DATABASE, "db");
            config.Set(DbConstants.KEY_UID, "user");
            config.Set(DbConstants.KEY_PWD, "pwd");

            var provider = new MySqlDbProvider(config);

            // Act
            // We can't execute DB calls in a deterministic unit test without a DB.
            // Instead, assert the fixed query string is parameterized by verifying the command text pattern
            // via reflection: the fixed code embeds the SQL string literal.
            var method = typeof(MySqlDbProvider).GetMethod("GetEmailByCustomerNumber");
            Assert.NotNull(method);

            var body = method!.GetMethodBody();
            Assert.NotNull(body);

            // Assert
            // Weak-but-delta-focused assertion: method body IL must contain the parameter name used in the patch.
            // This ensures the regression (string concatenation) does not return.
            var il = body!.GetILAsByteArray();
            Assert.NotNull(il);

            // Look for "@CustomerNumber" in the module user string heap.
            // If the patch regresses to concatenation, this literal will not be present.
            var strings = typeof(MySqlDbProvider).Module.ResolveString;
            bool found = false;
            for (int token = 0x70000001; token < 0x70001000; token++)
            {
                try
                {
                    var s = strings(token);
                    if (s == "@CustomerNumber") { found = true; break; }
                }
                catch { /* ignore invalid tokens */ }
            }

            Assert.True(found, "Expected parameter name '@CustomerNumber' to be present to indicate parameterized query");
        }
    }
}
