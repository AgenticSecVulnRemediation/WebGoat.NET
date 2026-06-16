using System;
using System.Data;
using System.IO;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetEmailByCustomerNumberTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_WithInjectionPayload_DoesNotReturnEmail()
        {
            // Arrange
            var dbFile = Path.Combine(Path.GetTempPath(), $"goatdb_{Guid.NewGuid():N}.sqlite");
            try
            {
                var cfg = new ConfigFile();
                cfg.Set(DbConstants.KEY_FILE_NAME, dbFile);
                cfg.Set(DbConstants.KEY_CLIENT_EXEC, "sqlite3");

                var provider = new SqliteDbProvider(cfg);

                using (var conn = new Mono.Data.Sqlite.SqliteConnection($"Data Source={dbFile};Version=3"))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "CREATE TABLE CustomerLogin (customerNumber TEXT, email TEXT);";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "INSERT INTO CustomerLogin(customerNumber, email) VALUES ('1', 'one@x.com');";
                        cmd.ExecuteNonQuery();
                    }
                }

                // Act: would have returned the first row when concatenated.
                var email = provider.GetEmailByCustomerNumber("1 OR 1=1");

                // Assert: should not return existing email for injected expression
                Assert.NotEqual("one@x.com", email);
            }
            finally
            {
                if (File.Exists(dbFile))
                    File.Delete(dbFile);
            }
        }
    }
}
