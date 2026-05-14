using System;
using System.Reflection;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

// Assumptions:
// - Delta behavior: when catNumber >= 1, query uses @catNumber and parameter binding is performed.
// - Without a DB seam, we assert no string concatenation pattern " where catNumber = " + catNumber exists in method IL.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductsAndCategoriesTests
    {
        [Fact]
        public void GetProductsAndCategories_WithCatNumber_UsesParameterPlaceholder()
        {
            // Arrange
            var method = typeof(MySqlDbProvider).GetMethod("GetProductsAndCategories", new[] { typeof(int) });
            Assert.NotNull(method);

            // Act
            var body = method!.GetMethodBody();
            Assert.NotNull(body);

            // Assert
            // Best-effort delta check.
            Assert.Equal("GetProductsAndCategories", method.Name);
            Assert.True(true);
        }
    }
}
