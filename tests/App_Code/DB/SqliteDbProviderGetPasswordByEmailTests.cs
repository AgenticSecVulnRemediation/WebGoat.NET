using System;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPasswordByEmailTests
    {
        [Fact]
        public void GetPasswordByEmail_WithSqlInjectionPayload_DoesNotReturnInjectedRowPassword()
        {
            // Arrange
            // Use an in-memory sqlite DB file via SQLite connection string from provider config.
            // The provider uses Mono.Data.Sqlite and builds Data Source={file};Version=3.
            // We'll create a temp file DB with the expected schema subset.
            var dbFile = System.IO.Path.GetTempFileName();
            try
            {
                // Create schema and seed
                using (var conn = new Mono.Data.Sqlite.SqliteConnection($"Data Source={dbFile};Version=3"))
                {
                    conn.Open();
                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = "CREATE TABLE CustomerLogin (Email TEXT, Password TEXT);";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "INSERT INTO CustomerLogin(Email, Password) VALUES ('victim@example.com','ENC_VICTIM');";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "INSERT INTO CustomerLogin(Email, Password) VALUES ('attacker@example.com','ENC_ATTACKER');";
                    cmd.ExecuteNonQuery();
                }

                // Minimal ConfigFile stub: relies on DbConstants.KEY_FILE_NAME
                var config = new OWASP.WebGoat.NET.App_Code.DB.ConfigFile();
                // ConfigFile in project typically reads from disk; we can set values via reflection for unit test.
                // If this fails due to ConfigFile implementation, test will reveal it.
                var dictField = typeof(OWASP.WebGoat.NET.App_Code.DB.ConfigFile).GetField("_values", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (dictField != null)
                {
                    var dict = (System.Collections.Generic.Dictionary<string, string>)dictField.GetValue(config);
                    dict[DbConstants.KEY_FILE_NAME] = dbFile;
                    dict[DbConstants.KEY_CLIENT_EXEC] = "";
                }
                else
                {
                    // Fallback: attempt to set through indexer/setter if available
                    var setMethod = typeof(OWASP.WebGoat.NET.App_Code.DB.ConfigFile).GetMethod("Set", new[] { typeof(string), typeof(string) });
                    if (setMethod != null)
                    {
                        setMethod.Invoke(config, new object[] { DbConstants.KEY_FILE_NAME, dbFile });
                        setMethod.Invoke(config, new object[] { DbConstants.KEY_CLIENT_EXEC, "" });
                    }
                }

                var provider = new SqliteDbProvider(config);

                // Act
                // Payload that previously would have turned WHERE clause into always-true.
                var payloadEmail = "victim@example.com' OR '1'='1";
                var result = provider.GetPasswordByEmail(payloadEmail);

                // Assert
                // After fix, query is parameterized and should not match any row; provider returns "Email Address Not Found!".
                Assert.Equal("Email Address Not Found!", result);
            }
            finally
            {
                try { System.IO.File.Delete(dbFile); } catch { /* ignore */ }
            }
        }
    }
}
