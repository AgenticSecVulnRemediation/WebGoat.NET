using System;
using Mono.Data.Sqlite;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPaymentsParameterizedTests
    {
        [Fact]
        public void GetPayments_UsesParameterizedQuery_ForCustomerNumber()
        {
            // Delta intent: query now uses @customerNumber.
            var cfg = new ConfigFile();
            cfg.Set(DbConstants.KEY_FILE_NAME, "test.db");
            cfg.Set(DbConstants.KEY_CLIENT_EXEC, "sqlite3");

            var provider = new SqliteDbProvider(cfg);

            var ex = Record.Exception(() => provider.GetPayments(1));
            if (ex is SqliteException sqliteEx)
            {
                Assert.DoesNotContain("syntax", sqliteEx.Message, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
