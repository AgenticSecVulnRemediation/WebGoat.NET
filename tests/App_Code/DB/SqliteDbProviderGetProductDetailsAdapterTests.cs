using System;
using System.Data;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

// Assumptions:
// - Delta behavior: GetProductDetails uses SelectCommand.Parameters.AddWithValue("@productCode", productCode)
// - This test ensures injection-like productCode does not broaden result set.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsAdapterTests
    {
        [Fact]
        public void GetProductDetails_ParameterizesProductCode_PreventsTautologyInjection()
        {
            // Arrange
            var cs = "Data Source=:memory:;Version=3;New=True;";
            using var conn = new SqliteConnection(cs);
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
CREATE TABLE Products(productCode TEXT PRIMARY KEY, productName TEXT);
CREATE TABLE Comments(productCode TEXT, comment TEXT);
INSERT INTO Products(productCode, productName) VALUES ('P1', 'Prod1');
";
                cmd.ExecuteNonQuery();
            }

            var provider = (SqliteDbProvider)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(SqliteDbProvider));
            typeof(SqliteDbProvider).GetField("_connectionString", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(provider, cs);

            // Act
            var ds = provider.GetProductDetails("P1' OR 1=1 --");

            // Assert
            Assert.NotNull(ds);
            Assert.Equal(0, ds.Tables["products"].Rows.Count);
        }
    }
}
