using System;
using System.Collections.Specialized;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderCustomerEmailsTests
    {
        [Fact]
        public void GetCustomerEmails_UsesParameterForLikeClause_AllowsQuotesWithoutSqlError()
        {
            // Arrange
            var nvc = new NameValueCollection
            {
                [DbConstants.KEY_FILE_NAME] = ":memory:",
                [DbConstants.KEY_CLIENT_EXEC] = "sqlite3"
            };
            var provider = new SqliteDbProvider(new ConfigFile(nvc));

            using (var conn = new SqliteConnection("Data Source=:memory:;Version=3"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE CustomerLogin(email TEXT); INSERT INTO CustomerLogin(email) VALUES ('a@b.com');";
                    cmd.ExecuteNonQuery();
                }

                // Inject provider connection string to use our in-memory db.
                var csField = typeof(SqliteDbProvider).GetField("_connectionString", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                Assert.NotNull(csField);
                csField!.SetValue(provider, conn.ConnectionString);

                // Act
                var ex = Record.Exception(() => provider.GetCustomerEmails("a@b.com' OR '1'='1"));

                // Assert
                Assert.Null(ex);
            }
        }
    }
}
