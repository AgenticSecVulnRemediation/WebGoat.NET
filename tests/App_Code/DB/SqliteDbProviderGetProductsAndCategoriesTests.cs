using System;
using System.Reflection;
using Xunit;

// Assumption: production code resides in namespace OWASP.WebGoat.NET.App_Code.DB
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductsAndCategoriesTests
    {
        [Fact]
        public void GetProductsAndCategories_OverloadExists_AndAcceptsCategoryNumber()
        {
            // Delta behavior: optional category filter should be parameterized (@catNumber) when catNumber >= 1.
            // Deterministic guard: the overload must exist.
            var method = typeof(SqliteDbProvider).GetMethod("GetProductsAndCategories", new[] { typeof(int) });
            Assert.NotNull(method);
        }
    }
}
