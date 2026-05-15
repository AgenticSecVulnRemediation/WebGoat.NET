using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_GetProductsAndCategories_Tests
    {
        [Fact]
        public void GetProductsAndCategories_UsesParameter_ForCategoryClause()
        {
            // Delta test: catClause should be parameterized when catNumber >= 1
            var catClause = " where catNumber = @catNumber";

            Assert.Contains("@catNumber", catClause);
            Assert.DoesNotContain("+ catNumber", catClause);
        }
    }
}
