using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductsAndCategoriesTests
    {
        [Fact]
        public void GetProductsAndCategories_MethodExists_AfterParameterizationChange()
        {
            // Delta guard: method still exists and compiles after adding parameterized queries for catNumber.
            var method = typeof(SqliteDbProvider).GetMethod(nameof(SqliteDbProvider.GetProductsAndCategories), new[] { typeof(int) });
            Assert.NotNull(method);
        }
    }
}
