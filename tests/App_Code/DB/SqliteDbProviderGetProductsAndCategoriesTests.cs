using System;
using System.Data;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

// Assumptions:
// - Delta behavior: when catNumber >= 1, query uses @catNumber parameter and doesn't concatenate.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductsAndCategoriesTests
    {
        [Fact]
        public void GetProductsAndCategories_WithCatNumber_DoesNotReturnAllRowsForInjection()
        {
            // Arrange
            var cs = "Data Source=:memory:;Version=3;New=True;";
            using var conn = new SqliteConnection(cs);
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
CREATE TABLE Categories(catNumber INTEGER PRIMARY KEY, name TEXT);
CREATE TABLE Products(catNumber INTEGER, productCode TEXT);
INSERT INTO Categories(catNumber, name) VALUES (1,'c1'), (2,'c2');
INSERT INTO Products(catNumber, productCode) VALUES (1,'p1'), (2,'p2');
";
                cmd.ExecuteNonQuery();
            }

            var provider = (SqliteDbProvider)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(SqliteDbProvider));
            typeof(SqliteDbProvider).GetField("_connectionString", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(provider, cs);

            // Act
            var ds = provider.GetProductsAndCategories(1);

            // Assert
            Assert.NotNull(ds);
            Assert.True(ds.Tables.Contains("categories"));
            Assert.True(ds.Tables.Contains("products"));
            Assert.All(ds.Tables["categories"].AsEnumerable(), r => Assert.Equal(1, Convert.ToInt32(r["catNumber"])));
            Assert.All(ds.Tables["products"].AsEnumerable(), r => Assert.Equal(1, Convert.ToInt32(r["catNumber"])));
        }
    }
}
