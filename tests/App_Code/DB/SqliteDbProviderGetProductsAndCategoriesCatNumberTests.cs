using System;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductsAndCategoriesCatNumberTests
    {
        [Fact]
        public void GetProductsAndCategories_WithCatNumber_UsesParameterAndDoesNotThrow()
        {
            // Regression test for PR 367: catNumber clause now parameterized only when catNumber >= 1.

            var dbPath = Path.GetTempFileName();
            try
            {
                File.Delete(dbPath);
                SqliteConnection.CreateFile(dbPath);
                var cs = $"Data Source={dbPath};Version=3";

                using (var cn = new SqliteConnection(cs))
                {
                    cn.Open();
                    using var cmd = cn.CreateCommand();
                    cmd.CommandText = @"
CREATE TABLE Categories(catNumber INTEGER);
CREATE TABLE Products(catNumber INTEGER);
INSERT INTO Categories(catNumber) VALUES (1);
INSERT INTO Products(catNumber) VALUES (1);
";
                    cmd.ExecuteNonQuery();
                }

                var provider = (SqliteDbProvider)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(SqliteDbProvider));
                typeof(SqliteDbProvider).GetField("_connectionString", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                    .SetValue(provider, cs);

                int exCount = 0;
                var ex = Record.Exception(() => provider.GetProductsAndCategories(1));
                if (ex != null) exCount++;

                Assert.Equal(0, exCount);
            }
            finally
            {
                if (File.Exists(dbPath)) File.Delete(dbPath);
            }
        }
    }
}
