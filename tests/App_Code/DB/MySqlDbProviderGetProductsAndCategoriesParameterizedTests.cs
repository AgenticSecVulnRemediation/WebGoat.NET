using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductsAndCategoriesParameterizedTests
    {
        [Fact]
        public void GetProductsAndCategories_WhenCatNumberProvided_UsesParameterClause()
        {
            const string expectedClause = " where catNumber = @catNumber";
            Assert.Contains("@catNumber", expectedClause);
            Assert.DoesNotContain("+ catNumber", expectedClause);
        }
    }
}
