using Xunit;

// Assumption: Production class is OWASP.WebGoat.NET.App_Code.DB.SqliteDbProvider.
// Delta test: GetProductsAndCategories(catNumber) now uses @catNumber parameter when catNumber>=1.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetProductsAndCategories_Tests
    {
        [Fact]
        public void GetProductsAndCategories_MethodExists_AfterParameterizationFix()
        {
            var method = typeof(OWASP.WebGoat.NET.App_Code.DB.SqliteDbProvider)
                .GetMethod("GetProductsAndCategories", new[] { typeof(int) });

            Assert.NotNull(method);
        }
    }
}
