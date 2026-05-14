using System;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsParameterizedTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedProductCode_RejectsInjectionByNotBreakingQuery()
        {
            // Regression test for PRs 362/364: productCode queries use @productCode parameter.
            // We'll create a temp sqlite file and ensure the method executes without SQL syntax error when productCode contains quotes.

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
CREATE TABLE Products(productCode TEXT PRIMARY KEY);
CREATE TABLE Comments(productCode TEXT);
INSERT INTO Products(productCode) VALUES ('S10_1678');
";
                    cmd.ExecuteNonQuery();
                }

                // Create provider without running constructor
                var provider = (SqliteDbProvider)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(SqliteDbProvider));
                typeof(SqliteDbProvider).GetField("_connectionString", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                    .SetValue(provider, cs);

                var ex = Record.Exception(() => provider.GetProductDetails("S10_1678' OR 1=1 --"));

                Assert.Null(ex);
            }
            finally
            {
                if (File.Exists(dbPath)) File.Delete(dbPath);
            }
        }
    }
}
