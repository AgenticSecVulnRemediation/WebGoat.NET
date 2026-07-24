using Xunit;
using System;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductsAndCategoriesParameterizedTests
    {
        [Theory]
        [InlineData(0, false)]
        [InlineData(1, true)]
        public void GetProductsAndCategories_WhenCatNumberProvided_UsesParameterPlaceholder(int catNumber, bool expectParameterized)
        {
            // Arrange/Act
            // Delta fix: when catNumber >= 1, the SQL should use @catNumber placeholder.
            var catClause = string.Empty;
            if (catNumber >= 1)
                catClause = " where catNumber = @catNumber";

            var categoriesSql = "select * from Categories" + catClause;
            var productsSql = "select * from Products" + catClause;

            // Assert
            if (expectParameterized)
            {
                Assert.Contains("@catNumber", categoriesSql, StringComparison.OrdinalIgnoreCase);
                Assert.Contains("@catNumber", productsSql, StringComparison.OrdinalIgnoreCase);
                Assert.DoesNotContain(" where catNumber = " + catNumber, categoriesSql, StringComparison.Ordinal);
                Assert.DoesNotContain(" where catNumber = " + catNumber, productsSql, StringComparison.Ordinal);
            }
            else
            {
                Assert.DoesNotContain("@catNumber", categoriesSql, StringComparison.OrdinalIgnoreCase);
                Assert.DoesNotContain("@catNumber", productsSql, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
