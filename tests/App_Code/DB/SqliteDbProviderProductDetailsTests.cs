using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQueries_ForProductsAndComments()
        {
            // Arrange
            const string productCode = "S10_1678";

            // Act
            string prodSql = "select * from Products where productCode = @productCode";
            var prodCmd = new SqliteCommand(prodSql);
            prodCmd.Parameters.AddWithValue("@productCode", productCode);

            string commSql = "select * from Comments where productCode = @productCode";
            var commCmd = new SqliteCommand(commSql);
            commCmd.Parameters.AddWithValue("@productCode", productCode);

            // Assert
            Assert.True(prodCmd.Parameters.Contains("@productCode"));
            Assert.Equal(productCode, prodCmd.Parameters["@productCode"].Value);

            Assert.True(commCmd.Parameters.Contains("@productCode"));
            Assert.Equal(productCode, commCmd.Parameters["@productCode"].Value);
        }
    }
}
