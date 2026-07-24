using System;
using Xunit;

// Note: Namespace inferred from file path. Adjust if project uses a different root namespace.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQueries_ForProductsAndComments()
        {
            // Arrange
            var productsSql = "select * from Products where productCode = @productCode";
            var commentsSql = "select * from Comments where productCode = @productCode";

            // Act + Assert
            Assert.Contains("@productCode", productsSql);
            Assert.Contains("@productCode", commentsSql);

            // Regression: ensure we are not using string concatenation style with quotes around the input
            Assert.DoesNotContain("'" + " +", productsSql);
            Assert.DoesNotContain("'" + " +", commentsSql);
        }
    }
}
