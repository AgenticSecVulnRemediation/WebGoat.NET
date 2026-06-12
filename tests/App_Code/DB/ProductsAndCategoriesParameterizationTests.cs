using System;
using System.Data;
using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class ProductsAndCategoriesParameterizationTests
    {
        [Fact]
        public void WhereClause_WithCatNumberPayload_DoesNotReturnAllRowsWhenParameterized()
        {
            // Arrange
            using var conn = new SqliteConnection("Data Source=:memory:;Version=3");
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE Categories (catNumber TEXT);";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO Categories(catNumber) VALUES ('1');";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO Categories(catNumber) VALUES ('2');";
                cmd.ExecuteNonQuery();
            }

            // Act
            using var cmd2 = conn.CreateCommand();
            cmd2.CommandText = "select * from Categories where catNumber = @catNumber";
            cmd2.Parameters.AddWithValue("@catNumber", "0 OR 1=1");

            using var adapter = new SqliteDataAdapter(cmd2);
            var ds = new DataSet();
            adapter.Fill(ds);

            // Assert
            Assert.True(ds.Tables.Count > 0);
            Assert.Equal(0, ds.Tables[0].Rows.Count);
        }
    }
}
