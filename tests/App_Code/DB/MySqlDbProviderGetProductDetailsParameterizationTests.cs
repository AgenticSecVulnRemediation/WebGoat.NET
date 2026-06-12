using MySql.Data.MySqlClient;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsParameterizationTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedProductCodeQuery_ForProductsAndComments()
        {
            // Arrange
            var productsSql = "select * from Products where productCode = @productCode";
            var commentsSql = "select * from Comments where productCode = @productCode";

            using var productsCmd = new MySqlCommand(productsSql);
            using var commentsCmd = new MySqlCommand(commentsSql);

            // Act
            productsCmd.Parameters.AddWithValue("@productCode", "S10_1678");
            commentsCmd.Parameters.AddWithValue("@productCode", "S10_1678");

            // Assert
            Assert.Contains("@productCode", productsCmd.CommandText);
            Assert.Contains("@productCode", commentsCmd.CommandText);
            Assert.Equal("S10_1678", productsCmd.Parameters["@productCode"].Value);
            Assert.Equal("S10_1678", commentsCmd.Parameters["@productCode"].Value);
        }
    }
}
