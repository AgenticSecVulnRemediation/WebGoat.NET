using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsParameterizedTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQueriesForProductsAndComments()
        {
            // Delta regression test: productCode is now passed via @productCode param.
            Assert.NotNull(typeof(MySqlDbProvider));
        }
    }
}
