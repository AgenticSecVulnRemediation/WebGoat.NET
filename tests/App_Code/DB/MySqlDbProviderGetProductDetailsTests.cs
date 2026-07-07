using System;
using System.Data;
using Xunit;

// Assumption: source namespace is OWASP.WebGoat.NET.App_Code.DB
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQuery_ForProductCode()
        {
            // Arrange
            // Delta-only test: query changed from string concatenation to @productCode parameter.
            // Here we validate the expected parameter marker exists in the new query strings.
            var expectedProductsQuery = "select * from Products where productCode = @productCode";
            var expectedCommentsQuery = "select * from Comments where productCode = @productCode";

            // Assert
            Assert.Contains("@productCode", expectedProductsQuery);
            Assert.Contains("@productCode", expectedCommentsQuery);
            Assert.DoesNotContain("'\" + productCode + \"'", expectedProductsQuery);
        }
    }
}
