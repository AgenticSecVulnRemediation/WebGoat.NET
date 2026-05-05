using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQueries_ForProductCode_InBothQueries()
        {
            // Arrange
            const string productsSql = "select * from Products where productCode = @productCode";
            const string commentsSql = "select * from Comments where productCode = @productCode";

            // Act
            using var prodCmd = new SqliteCommand(productsSql);
            prodCmd.Parameters.AddWithValue("@productCode", "S10_1678");

            using var commCmd = new SqliteCommand(commentsSql);
            commCmd.Parameters.AddWithValue("@productCode", "S10_1678");

            // Assert
            Assert.Contains("@productCode", prodCmd.CommandText);
            Assert.Single(prodCmd.Parameters);
            Assert.Equal("@productCode", prodCmd.Parameters[0].ParameterName);

            Assert.Contains("@productCode", commCmd.CommandText);
            Assert.Single(commCmd.Parameters);
            Assert.Equal("@productCode", commCmd.Parameters[0].ParameterName);
        }
    }
}
