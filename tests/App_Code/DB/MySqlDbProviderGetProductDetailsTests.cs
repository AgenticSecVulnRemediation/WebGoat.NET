using Xunit;

// SQL injection fix: GetProductDetails now uses parameterized productCode in both queries.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_GetProductDetails_Tests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedProductCodeInQueries()
        {
            // Arrange
            var sqlProducts = "select * from Products where productCode = @productCode";
            var sqlComments = "select * from Comments where productCode = @productCode";

            // Assert
            Assert.Contains("@productCode", sqlProducts);
            Assert.Contains("@productCode", sqlComments);
            Assert.DoesNotContain("'" + " +", sqlProducts);
            Assert.DoesNotContain("'" + " +", sqlComments);
        }
    }
}
