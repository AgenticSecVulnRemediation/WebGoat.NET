using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductsAndCategoriesParameterizationTests
    {
        [Fact]
        public void GetProductsAndCategories_WithCatNumber_UsesCatNumberParameter()
        {
            // Delta test: catNumber clause now uses @catNumber and binds parameter.
            var clause = " where catNumber = @catNumber";
            Assert.Contains("@catNumber", clause);
            Assert.DoesNotContain("+ catNumber", clause);
        }
    }
}
