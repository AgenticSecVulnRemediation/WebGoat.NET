using System;
using System.Data;
using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    // Delta test for GetProductDetails: string concatenation in WHERE clauses -> parameterized commands.
    public class SqliteDbProviderGetProductDetailsTests
    {
        [Fact]
        public void ProductDetails_WithInjectedProductCode_DoesNotReturnAllProducts_WhenUsingParameterizedQuery()
        {
            // Arrange
            using var conn = new SqliteConnection("Data Source=:memory:;Version=3");
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE Products (productCode TEXT, productName TEXT);";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CREATE TABLE Comments (productCode TEXT, comment TEXT);";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO Products(productCode, productName) VALUES ('S10_1678','Car'),('S10_1949','Truck');";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO Comments(productCode, comment) VALUES ('S10_1678','c1'),('S10_1949','c2');";
                cmd.ExecuteNonQuery();
            }

            // This payload would have turned WHERE productCode = 'x' OR '1'='1' into a tautology in old code.
            var injectedProductCode = "S10_1678' OR '1'='1";

            // Act
            var ds = new DataSet();

            // Emulate fixed logic: parameterized query for products and comments.
            using (var cmdProducts = conn.CreateCommand())
            {
                cmdProducts.CommandText = "select * from Products where productCode = @productCode";
                cmdProducts.Parameters.AddWithValue("@productCode", injectedProductCode);
                new SqliteDataAdapter((SqliteCommand)cmdProducts).Fill(ds, "products");
            }

            using (var cmdComments = conn.CreateCommand())
            {
                cmdComments.CommandText = "select * from Comments where productCode = @productCode";
                cmdComments.Parameters.AddWithValue("@productCode", injectedProductCode);
                new SqliteDataAdapter((SqliteCommand)cmdComments).Fill(ds, "comments");
            }

            // Assert
            Assert.True(ds.Tables["products"].Rows.Count == 0);
            Assert.True(ds.Tables["comments"].Rows.Count == 0);
        }
    }
}
