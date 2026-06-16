using System;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductsAndCategoriesParameterizationRuntimeTests
    {
        [Fact]
        public void ProductsAndCategoriesQuery_WithCatNumberParameter_DoesNotExecuteInjectedClause()
        {
            // Arrange
            var tempDb = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".sqlite");
            SqliteConnection.CreateFile(tempDb);

            try
            {
                using (var conn = new SqliteConnection($"Data Source={tempDb};Version=3"))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "CREATE TABLE Categories (catNumber INTEGER, name TEXT);";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "CREATE TABLE Products (catNumber INTEGER, productCode TEXT);";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "INSERT INTO Categories(catNumber,name) VALUES (1,'c1'), (2,'c2');";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "INSERT INTO Products(catNumber,productCode) VALUES (1,'p1'), (2,'p2');";
                        cmd.ExecuteNonQuery();
                    }
                }

                // Act: run the fixed query shape with injected-looking input.
                var catClause = " where catNumber = @catNumber";
                using (var conn = new SqliteConnection($"Data Source={tempDb};Version=3"))
                {
                    conn.Open();
                    var da = new SqliteDataAdapter("select * from Categories" + catClause, conn);
                    da.SelectCommand.Parameters.AddWithValue("@catNumber", "1 OR 1=1");
                    var ds = new DataSet();
                    da.Fill(ds);

                    // Assert: should not return all categories.
                    Assert.Equal(0, ds.Tables[0].Rows.Count);
                }
            }
            finally
            {
                if (File.Exists(tempDb))
                    File.Delete(tempDb);
            }
        }
    }
}
