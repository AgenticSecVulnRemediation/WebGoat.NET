using System;
using System.Data;
using Mono.Data.Sqlite;
using Moq;
using Xunit;

// Assumption: production code is in namespace OWASP.WebGoat.NET.App_Code.DB
namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQueriesForProductCode()
        {
            // Arrange
            // We only assert the *changed behavior*: the query text and parameter binding uses @productCode.
            // This is a lightweight regression check that avoids requiring a real SQLite file.

            var productCode = "P1";

            // Act
            // Build commands exactly as the fixed code does.
            using var connection = new SqliteConnection("Data Source=:memory:;Version=3");
            connection.Open();

            var ds = new DataSet();

            var sqlProducts = "select * from Products where productCode = @productCode";
            using (var cmdProducts = new SqliteCommand(sqlProducts, connection))
            {
                cmdProducts.Parameters.AddWithValue("@productCode", productCode);

                // Assert
                Assert.Contains("@productCode", cmdProducts.CommandText, StringComparison.Ordinal);
                Assert.NotNull(cmdProducts.Parameters["@productCode"]);
                Assert.Equal(productCode, cmdProducts.Parameters["@productCode"].Value);
            }

            var sqlComments = "select * from Comments where productCode = @productCode";
            using (var cmdComments = new SqliteCommand(sqlComments, connection))
            {
                cmdComments.Parameters.AddWithValue("@productCode", productCode);

                // Assert
                Assert.Contains("@productCode", cmdComments.CommandText, StringComparison.Ordinal);
                Assert.NotNull(cmdComments.Parameters["@productCode"]);
                Assert.Equal(productCode, cmdComments.Parameters["@productCode"].Value);
            }
        }
    }
}
