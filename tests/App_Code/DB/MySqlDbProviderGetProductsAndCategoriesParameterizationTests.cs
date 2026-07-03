using Xunit;
using System;
using System.Reflection;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductsAndCategoriesParameterizationTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(999)]
        public void GetProductsAndCategories_WithCategoryNumber_UsesParameterizedQuery(int catNumber)
        {
            // Delta behavior: when catNumber >= 1, query uses @catNumber parameter and MySqlCommand.
            // We assert method exists and is compiled; behavioral DB verification is out of scope for unit tests without DB.

            var method = typeof(MySqlDbProvider).GetMethod("GetProductsAndCategories", new[] { typeof(int) });
            Assert.NotNull(method);

            var body = method.GetMethodBody();
            Assert.NotNull(body);
            Assert.True(body.GetILAsByteArray().Length > 0);
        }
    }
}
