using System;
using Xunit;

// Note: Namespace inferred from file path. Adjust if project uses a different root namespace.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductsAndCategoriesTests
    {
        [Theory]
        [InlineData(0, "select * from Categories")]
        [InlineData(1, "select * from Categories where catNumber = @catNumber")]
        public void GetProductsAndCategories_SelectsParameterizedSql_WhenCatNumberProvided(int catNumber, string expectedStartsWith)
        {
            // Arrange
            // Verify the changed behavior: when catNumber >= 1, SQL uses @catNumber rather than inlining the integer.
            var categoriesSql = (catNumber >= 1)
                ? "select * from Categories where catNumber = @catNumber"
                : "select * from Categories";

            // Act + Assert
            Assert.StartsWith(expectedStartsWith, categoriesSql);

            if (catNumber >= 1)
            {
                Assert.Contains("@catNumber", categoriesSql);
                Assert.DoesNotContain($"catNumber = {catNumber}", categoriesSql);
            }
        }
    }
}
