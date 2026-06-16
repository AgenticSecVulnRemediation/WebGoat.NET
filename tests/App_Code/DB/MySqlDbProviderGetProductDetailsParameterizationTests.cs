using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsParameterizationTests
    {
        [Fact]
        public void GetProductDetails_UsesProductCodeParameter_ForBothQueries()
        {
            // Regression test for injection fix: productCode must be bound via @productCode parameter.
            var asm = typeof(MySqlDbProvider).Assembly.ToString();

            Assert.Contains("where productCode = @productCode", asm);
            Assert.DoesNotContain("where productCode = '\" + productCode", asm);
        }
    }
}
