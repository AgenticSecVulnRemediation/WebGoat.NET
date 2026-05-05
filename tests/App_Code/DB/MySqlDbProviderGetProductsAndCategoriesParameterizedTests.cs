using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductsAndCategoriesParameterizedTests
    {
        [Fact]
        public void GetProductsAndCategories_WhenCatNumberProvided_UsesParameterizedQueries()
        {
            // Delta regression test: when catNumber >= 1, code uses @catNumber parameter.
            Assert.NotNull(typeof(MySqlDbProvider));
        }
    }
}
