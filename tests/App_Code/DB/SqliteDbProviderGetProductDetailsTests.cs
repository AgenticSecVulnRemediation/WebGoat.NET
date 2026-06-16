using System;
using System.Data;
using System.IO;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_WithInjectionPayload_DoesNotReturnUnrelatedRows()
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
                        cmd.CommandText = "CREATE TABLE Products (productCode TEXT, productName TEXT);";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "CREATE TABLE Comments (productCode TEXT, comment TEXT);";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "INSERT INTO Products(productCode, productName) VALUES ('S10_1678', 'Car');";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "INSERT INTO Products(productCode, productName) VALUES ('S10_1949', 'Truck');";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "INSERT INTO Comments(productCode, comment) VALUES ('S10_1678', 'Nice');";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "INSERT INTO Comments(productCode, comment) VALUES ('S10_1949', 'Cool');";
                        cmd.ExecuteNonQuery();
                    }
                }

                // Act: previously concatenated query would return all products for payload.
                var ds = provider.GetProductDetails("S10_1678' OR '1'='1");

                // Assert: secure behavior should not match any product rows because exact value doesn't exist.
                Assert.NotNull(ds);
                Assert.True(ds.Tables.Contains("products"));
                Assert.True(ds.Tables.Contains("comments"));
                Assert.Equal(0, ds.Tables["products"].Rows.Count);
                Assert.Equal(0, ds.Tables["comments"].Rows.Count);
            }
            finally
            {
                if (File.Exists(dbFile))
                    File.Delete(dbFile);
            }
        }
    }
}
