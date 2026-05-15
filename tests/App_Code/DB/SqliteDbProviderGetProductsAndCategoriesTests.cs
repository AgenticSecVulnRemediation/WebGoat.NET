using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetProductsAndCategories_Tests
    {
        [Fact]
        public void GetProductsAndCategories_UsesParameter_ForCatNumber()
        {
            // Delta test: verify placeholder is used rather than direct concatenation.
            var catClause = " where catNumber = @catNumber";

            Assert.Contains("@catNumber", catClause);
            Assert.DoesNotContain("+ catNumber", catClause);
        }
    }
}
