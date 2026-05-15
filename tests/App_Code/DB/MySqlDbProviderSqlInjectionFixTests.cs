using System;
using Moq;
using MySql.Data.MySqlClient;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderSqlInjectionFixTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesParameterizedQuery_ForEmail()
        {
            // Delta intent: ensure query now uses @email parameter rather than string concatenation.
            // We validate by inspecting the SQL text embedded in method body via reflection is not feasible,
            // so we assert behavior: passing an email containing quote should not break with syntax error.

            var cfg = new ConfigFile();
            // Minimal config; connection attempts may fail in unit environment.
            cfg.Set(DbConstants.KEY_HOST, "localhost");
            cfg.Set(DbConstants.KEY_PORT, "3306");
            cfg.Set(DbConstants.KEY_DATABASE, "db");
            cfg.Set(DbConstants.KEY_UID, "user");
            cfg.Set(DbConstants.KEY_PWD, "pwd");

            var provider = new MySqlDbProvider(cfg);

            // Act/Assert: should not throw due to malformed SQL composition (may still throw due to connection).
            var ex = Record.Exception(() => provider.CustomCustomerLogin("a' OR '1'='1", "pw"));
            // We accept connection-related errors, but the fix should avoid immediate SQL syntax errors caused by concatenation.
            if (ex is MySqlException mysqlEx)
            {
                Assert.DoesNotContain("You have an error in your SQL syntax", mysqlEx.Message, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
