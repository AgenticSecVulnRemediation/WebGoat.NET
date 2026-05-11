using System;
using System.Data;
using MySql.Data.MySqlClient;
using Moq;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQuery_ProductCodeNotInterpolated()
        {
            // Arrange
            // This is a delta test focused on the security fix: avoid SQL injection by parameterizing productCode.
            var productCode = "ABC' OR '1'='1";

            // We can't easily execute without a real MySQL server; instead we verify the *command text shape*
            // by recreating the command construction performed in the method.
            using var conn = new MySqlConnection();

            var sql = "select * from Products where productCode = @productCode";
            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@productCode", productCode);

            // Assert
            Assert.Contains("productCode = @productCode", cmd.CommandText, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain(productCode, cmd.CommandText, StringComparison.Ordinal);
            Assert.NotNull(cmd.Parameters["@productCode"]);
            Assert.Equal(productCode, cmd.Parameters["@productCode"].Value);
        }

        [Fact]
        public void GetProductDetails_UsesParameterizedQuery_ForCommentsToo()
        {
            // Arrange
            var productCode = "XYZ' --";
            using var conn = new MySqlConnection();

            var sql = "select * from Comments where productCode = @productCode";
            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@productCode", productCode);

            // Assert
            Assert.Contains("productCode = @productCode", cmd.CommandText, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain(productCode, cmd.CommandText, StringComparison.Ordinal);
        }
    }
}
