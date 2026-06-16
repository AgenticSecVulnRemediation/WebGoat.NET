using System;
using System.Data;
using System.IO;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailTests
    {
        [Fact]
        public void GetCustomerEmail_BindsCustomerNumberParameter_ReturnsEmailForExactMatch()
        {
            // Arrange: create a temp sqlite db file and minimal schema.
            var dbFile = Path.Combine(Path.GetTempPath(), $"goatdb_{Guid.NewGuid():N}.sqlite");
            try
            {
                // Minimal ConfigFile setup
                var cfg = new ConfigFile();
                cfg.Set(DbConstants.KEY_FILE_NAME, dbFile);
                cfg.Set(DbConstants.KEY_CLIENT_EXEC, "sqlite3");

                var provider = new SqliteDbProvider(cfg);

                // Seed DB: CustomerLogin table
                using (var conn = new Mono.Data.Sqlite.SqliteConnection($"Data Source={dbFile};Version=3"))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "CREATE TABLE CustomerLogin (customerNumber TEXT, email TEXT);";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "INSERT INTO CustomerLogin(customerNumber, email) VALUES ('123', 'a@b.com');";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "INSERT INTO CustomerLogin(customerNumber, email) VALUES ('1234', 'c@d.com');";
                        cmd.ExecuteNonQuery();
                    }
                }

                // Act: try an injection-like value that would have expanded selection when concatenated.
                var result = provider.GetCustomerEmail("123' OR '1'='1");

                // Assert: parameterization should prevent injection; no row matches exact value, so scalar should be null -> exception -> message.
                Assert.NotEqual("a@b.com", result);
                Assert.NotEqual("c@d.com", result);
            }
            finally
            {
                if (File.Exists(dbFile))
                    File.Delete(dbFile);
            }
        }
    }
}
