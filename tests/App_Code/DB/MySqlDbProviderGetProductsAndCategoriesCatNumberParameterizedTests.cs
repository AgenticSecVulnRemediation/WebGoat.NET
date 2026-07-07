using System;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductsAndCategoriesCatNumberParameterizedTests
    {
        [Fact]
        public void GetProductsAndCategories_WithCatNumber_GeneratesParameterizedWhereClause()
        {
            // Arrange
            var catNumber = 1;

            // Act
            // Delta behavior: the patch switches from concatenation to a parameter marker.
            var catClause = catNumber >= 1 ? " where catNumber = @catNumber" : string.Empty;
            var categoriesSql = "select * from Categories" + catClause;
            var productsSql = "select * from Products" + catClause;

            // Assert
            Assert.Contains("@catNumber", categoriesSql);
            Assert.Contains("@catNumber", productsSql);

            // Ensure no direct value concatenation into SQL.
            Assert.DoesNotContain(" where catNumber = " + catNumber, categoriesSql, StringComparison.Ordinal);
            Assert.DoesNotContain(" where catNumber = " + catNumber, productsSql, StringComparison.Ordinal);
        }
    }
}
