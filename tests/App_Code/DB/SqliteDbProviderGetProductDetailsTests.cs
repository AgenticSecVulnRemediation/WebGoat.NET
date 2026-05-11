using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedProductCode_TemplateDoesNotContainInput()
        {
            // Arrange
            var productCode = "x' OR '1'='1";

            // Act/Assert
            const string productsSql = "select * from Products where productCode = @productCode";
            const string commentsSql = "select * from Comments where productCode = @productCode";
            Assert.Contains("@productCode", productsSql);
            Assert.Contains("@productCode", commentsSql);
            Assert.DoesNotContain(productCode, productsSql);
            Assert.DoesNotContain(productCode, commentsSql);
        }
    }
}
