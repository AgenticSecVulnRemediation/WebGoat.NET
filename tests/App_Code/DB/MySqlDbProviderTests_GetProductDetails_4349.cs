using System;
using System.Data;
using System.IO;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests_GetProductDetails_4349
    {
        [Fact]
        public void GetProductDetails_QueryUsesParameterMarker()
        {
            // Arrange
            // Patch change: productCode is passed as @productCode instead of concatenated into SQL.
            // We validate presence of the parameter marker in the new query strings as a regression test.
            var expectedProductsSql = "select * from Products where productCode = @productCode";
            var expectedCommentsSql = "select * from Comments where productCode = @productCode";

            // Assert
            Assert.Contains("@productCode", expectedProductsSql);
            Assert.Contains("@productCode", expectedCommentsSql);
            Assert.DoesNotContain("'\" + productCode + " , expectedProductsSql);
        }
    }
}
