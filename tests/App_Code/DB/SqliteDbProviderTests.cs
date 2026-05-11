using System;
using System.Collections.Specialized;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void AddComment_UsesParameters_DoesNotInlineValuesInSql()
        {
            // Arrange
            var nvc = new NameValueCollection
            {
                [DbConstants.KEY_FILE_NAME] = ":memory:",
                [DbConstants.KEY_CLIENT_EXEC] = "sqlite3"
            };

            var provider = new SqliteDbProvider(new ConfigFile(nvc));

            // Act
            // We use an in-memory SQLite db to avoid external dependencies.
            // Create minimal Comments table and verify that malicious values are inserted as data.
            using (var conn = new SqliteConnection("Data Source=:memory:;Version=3"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE Comments(productCode TEXT, email TEXT, comment TEXT);";
                    cmd.ExecuteNonQuery();
                }

                // Re-point provider's connection string via reflection to use this in-memory connection.
                var csField = typeof(SqliteDbProvider).GetField("_connectionString", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                Assert.NotNull(csField);
                csField!.SetValue(provider, conn.ConnectionString);

                string productCode = "p1";
                string email = "a@b.com'; DROP TABLE Comments; --";
                string comment = "c'); DROP TABLE Comments; --";

                var err = provider.AddComment(productCode, email, comment);
                Assert.Null(err);

                using (var verify = conn.CreateCommand())
                {
                    verify.CommandText = "SELECT COUNT(*) FROM Comments";
                    var count = Convert.ToInt32(verify.ExecuteScalar());
                    Assert.Equal(1, count);
                }
            }
        }
    }
}
