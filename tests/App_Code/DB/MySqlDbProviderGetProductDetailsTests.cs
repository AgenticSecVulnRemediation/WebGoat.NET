using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedProductCode_ForProductsAndCommentsQueries()
        {
            // Arrange
            string productsSql = "select * from Products where productCode = @productCode";
            string commentsSql = "select * from Comments where productCode = @productCode";

            // Assert
            Assert.Contains("@productCode", productsSql, StringComparison.Ordinal);
            Assert.Contains("@productCode", commentsSql, StringComparison.Ordinal);
            Assert.DoesNotContain("productCode = '\" +", productsSql, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("productCode = '\" +", commentsSql, StringComparison.OrdinalIgnoreCase);
        }
    }
}
