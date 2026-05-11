using System;
using System.Collections.Specialized;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_WithInjectionLikeInput_DoesNotBreakSqlConstruction()
        {
            // Arrange
            var nvc = new NameValueCollection
            {
                [DbConstants.KEY_FILE_NAME] = ":memory:",
                [DbConstants.KEY_CLIENT_EXEC] = "sqlite3"
            };
            var provider = new SqliteDbProvider(new ConfigFile(nvc));

            // Use in-memory db and minimal schema to allow adapters to fill.
            using (var conn = new SqliteConnection("Data Source=:memory:;Version=3"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
CREATE TABLE Products(productCode TEXT, catNumber INTEGER);
CREATE TABLE Comments(productCode TEXT, email TEXT, comment TEXT);
INSERT INTO Products(productCode, catNumber) VALUES ('safe', 1);
INSERT INTO Comments(productCode, email, comment) VALUES ('safe', 'e', 'c');";
                    cmd.ExecuteNonQuery();
                }

                var csField = typeof(SqliteDbProvider).GetField("_connectionString", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                Assert.NotNull(csField);
                csField!.SetValue(provider, conn.ConnectionString);

                // Act
                var ex = Record.Exception(() => provider.GetProductDetails("safe' OR '1'='1"));

                // Assert
                Assert.Null(ex);
            }
        }
    }
}
