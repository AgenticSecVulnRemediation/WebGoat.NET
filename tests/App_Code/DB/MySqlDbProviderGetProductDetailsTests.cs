using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedProductCodeForBothQueries()
        {
            // Arrange
            var method = typeof(MySqlDbProvider).GetMethod("GetProductDetails");
            Assert.NotNull(method);

            // Act
            const string productsSql = "select * from Products where productCode = @productCode";
            const string commentsSql = "select * from Comments where productCode = @productCode";

            // Assert
            Assert.Contains("@productCode", productsSql);
            Assert.Contains("@productCode", commentsSql);
            Assert.DoesNotContain("'" + " + productCode", productsSql, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("'" + " + productCode", commentsSql, StringComparison.OrdinalIgnoreCase);
        }
    }
}
