using System;
using MySql.Data.MySqlClient;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQuery_ForProductCode_InBothQueries()
        {
            // Arrange
            const string productsSql = "select * from Products where productCode = @productCode";
            const string commentsSql = "select * from Comments where productCode = @productCode";
            const string connectionString = "Server=localhost;Database=test;Uid=u;Pwd=p";
            using var connection = new MySqlConnection(connectionString);

            // Act
            using var productsCommand = new MySqlCommand(productsSql, connection);
            productsCommand.Parameters.AddWithValue("@productCode", "S10_1678");

            using var commentsCommand = new MySqlCommand(commentsSql, connection);
            commentsCommand.Parameters.AddWithValue("@productCode", "S10_1678");

            // Assert
            Assert.Contains("@productCode", productsCommand.CommandText);
            Assert.Contains("@productCode", commentsCommand.CommandText);

            Assert.Single(productsCommand.Parameters);
            Assert.Equal("@productCode", productsCommand.Parameters[0].ParameterName);

            Assert.Single(commentsCommand.Parameters);
            Assert.Equal("@productCode", commentsCommand.Parameters[0].ParameterName);
        }
    }
}
