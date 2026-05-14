using System;
using System.Data;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

// Assumptions:
// - We can run the patched method against an in-memory sqlite database by overriding internal connection string via reflection.
// - Delta behavior: GetProductDetails uses parameterized queries for productCode.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_DoesNotBreakOnSqlInjectionLikeInput()
        {
            // Arrange
            var dbFile = ":memory:";
            var cs = "Data Source=:memory:;Version=3;New=True;";

            using var conn = new SqliteConnection(cs);
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
CREATE TABLE Products(productCode TEXT PRIMARY KEY, productName TEXT);
CREATE TABLE Comments(productCode TEXT, comment TEXT);
INSERT INTO Products(productCode, productName) VALUES ('S10_1678', 'Car');
INSERT INTO Comments(productCode, comment) VALUES ('S10_1678', 'ok');
";
                cmd.ExecuteNonQuery();
            }

            // Create provider and force its connection string to our in-memory one.
            var provider = (SqliteDbProvider)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(SqliteDbProvider));
            typeof(SqliteDbProvider).GetField("_connectionString", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(provider, cs);

            // Act
            // An injection-like value should not return all products; it should return none.
            var ds = provider.GetProductDetails("S10_1678' OR '1'='1");

            // Assert
            Assert.NotNull(ds);
            Assert.True(ds.Tables.Contains("products"));
            Assert.Equal(0, ds.Tables["products"].Rows.Count);
        }
    }
}
