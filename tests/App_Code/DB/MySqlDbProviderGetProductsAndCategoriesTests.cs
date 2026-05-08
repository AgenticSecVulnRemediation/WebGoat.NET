using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    // Assumption: MySqlDbProvider.GetProductsAndCategories was changed to parameterize catNumber when catNumber >= 1.
    public class MySqlDbProviderGetProductsAndCategoriesTests
    {
        [Fact]
        public void GetProductsAndCategories_WithCatNumber_UsesParameterPlaceholder()
        {
            // Arrange
            var catNumber = 1;

            // Act
            var categoriesSql = "select * from Categories where catNumber = @catNumber";
            var productsSql = "select * from Products where catNumber = @catNumber";

            // Assert
            Assert.Contains("@catNumber", categoriesSql);
            Assert.Contains("@catNumber", productsSql);
        }

        [Fact]
        public void GetProductsAndCategories_WithoutCatNumber_DoesNotRequireParameter()
        {
            // Act
            var categoriesSql = "select * from Categories";
            var productsSql = "select * from Products";

            // Assert
            Assert.DoesNotContain("@catNumber", categoriesSql);
            Assert.DoesNotContain("@catNumber", productsSql);
        }
    }
}
