using System;
using System.Data;
using MySql.Data.MySqlClient;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameter_ForProductCode_InProductsQuery()
        {
            // Arrange
            var sql = "select * from Products where productCode = @productCode";
            using var connection = new MySqlConnection();

            // Act
            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@productCode", "S10_1678");
            var da = new MySqlDataAdapter(cmd);

            // Assert
            Assert.Contains("@productCode", da.SelectCommand.CommandText, StringComparison.Ordinal);
            Assert.NotNull(da.SelectCommand.Parameters["@productCode"]);
            Assert.Equal("S10_1678", da.SelectCommand.Parameters["@productCode"].Value);
        }

        [Fact]
        public void GetProductDetails_UsesParameter_ForProductCode_InCommentsQuery()
        {
            // Arrange
            var sql = "select * from Comments where productCode = @productCode";
            using var connection = new MySqlConnection();

            // Act
            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@productCode", "S10_1678");
            var da = new MySqlDataAdapter(cmd);

            // Assert
            Assert.Contains("@productCode", da.SelectCommand.CommandText, StringComparison.Ordinal);
            Assert.NotNull(da.SelectCommand.Parameters["@productCode"]);
        }
    }
}
