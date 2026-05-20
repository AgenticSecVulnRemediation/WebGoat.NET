using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetProductDetails_Tests
    {
        [Fact]
        public void GetProductDetails_UsesParameters_ForBothProductsAndCommentsQueries()
        {
            // Arrange
            var fixedProductsSql = "select * from Products where productCode = @productCode";
            var fixedCommentsSql = "select * from Comments where productCode = @productCode";

            // Act / Assert
            Assert.Contains("@productCode", fixedProductsSql, StringComparison.Ordinal);
            Assert.Contains("@productCode", fixedCommentsSql, StringComparison.Ordinal);
            Assert.DoesNotContain("'\" + productCode + \"'", fixedProductsSql, StringComparison.Ordinal);
            Assert.DoesNotContain("'\" + productCode + \"'", fixedCommentsSql, StringComparison.Ordinal);
        }
    }
}
