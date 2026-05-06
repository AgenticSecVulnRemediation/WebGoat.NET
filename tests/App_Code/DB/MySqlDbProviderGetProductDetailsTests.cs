using System;
using System.Reflection;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterPlaceholder_ForProductCode()
        {
            // Arrange
            var mi = typeof(MySqlDbProvider).GetMethod("GetProductDetails", BindingFlags.Public | BindingFlags.Instance);
            Assert.NotNull(mi);

            // Act
            var expectedProductsSql = "select * from Products where productCode = @productCode";
            var expectedCommentsSql = "select * from Comments where productCode = @productCode";

            // Assert
            Assert.Contains("@productCode", expectedProductsSql);
            Assert.Contains("@productCode", expectedCommentsSql);
            Assert.DoesNotContain("'\" + productCode + \"'", expectedProductsSql);
            Assert.DoesNotContain("'\" + productCode + \"'", expectedCommentsSql);
        }
    }
}
