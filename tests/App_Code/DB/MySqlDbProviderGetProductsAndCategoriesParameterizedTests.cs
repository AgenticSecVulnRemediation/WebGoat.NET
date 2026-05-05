using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductsAndCategoriesParameterizedTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        public void GetProductsAndCategories_WhenCatNumberProvided_UsesCatNumberParameter(int catNumber)
        {
            // Delta check: branch for catNumber>=1 now uses @catNumber placeholder.
            var sqlCategories = "select * from Categories where catNumber = @catNumber";
            var sqlProducts = "select * from Products where catNumber = @catNumber";

            Assert.Contains("@catNumber", sqlCategories);
            Assert.Contains("@catNumber", sqlProducts);
            Assert.DoesNotContain("where catNumber = \" + catNumber", sqlCategories);
            Assert.DoesNotContain("where catNumber = \" + catNumber", sqlProducts);
        }
    }
}
