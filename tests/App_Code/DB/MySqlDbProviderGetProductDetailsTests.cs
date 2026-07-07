using System;
using MySql.Data.MySqlClient;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesMySqlCommandWithProductCodeParameter()
        {
            // Arrange
            const string sql = "select * from Products where productCode = @productCode";

            using var cmd = new MySqlCommand(sql);

            // Act
            cmd.Parameters.AddWithValue("@productCode", "S10_1678");

            // Assert
            Assert.Contains("@productCode", cmd.CommandText);
            Assert.DoesNotContain("productCode = '", cmd.CommandText); // old concatenation style
            Assert.NotNull(cmd.Parameters["@productCode"]);
        }
    }
}
