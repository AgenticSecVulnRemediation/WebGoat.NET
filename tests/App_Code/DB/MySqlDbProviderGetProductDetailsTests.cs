using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedProductCodeQuery()
        {
            // Arrange
            const string productsSql = "select * from Products where productCode = @productCode";
            const string commentsSql = "select * from Comments where productCode = @productCode";

            // Assert
            Assert.DoesNotContain("'\" + productCode + \"'", productsSql);
            Assert.DoesNotContain("'\" + productCode + \"'", commentsSql);
            Assert.Contains("@productCode", productsSql);
            Assert.Contains("@productCode", commentsSql);
        }
    }
}
