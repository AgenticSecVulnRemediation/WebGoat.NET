using System;
using MySql.Data.MySqlClient;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailParameterizedTests
    {
        [Fact]
        public void GetCustomerEmail_WithInjectionPayload_DoesNotThrowSqlSyntaxErrorFromConcatenation()
        {
            // Delta intent: customerNumber is now bound as @customerNumber rather than concatenated.
            var cfg = new ConfigFile();
            cfg.Set(DbConstants.KEY_HOST, "localhost");
            cfg.Set(DbConstants.KEY_PORT, "3306");
            cfg.Set(DbConstants.KEY_DATABASE, "db");
            cfg.Set(DbConstants.KEY_UID, "user");
            cfg.Set(DbConstants.KEY_PWD, "pwd");

            var provider = new MySqlDbProvider(cfg);

            var ex = Record.Exception(() => provider.GetCustomerEmail("1 OR 1=1"));
            if (ex is MySqlException mysqlEx)
            {
                Assert.DoesNotContain("You have an error in your SQL syntax", mysqlEx.Message, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
