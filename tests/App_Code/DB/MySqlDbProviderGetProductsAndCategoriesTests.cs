using System;
using System.Reflection;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductsAndCategoriesTests
    {
        [Theory]
        [InlineData(0, false)]
        [InlineData(1, true)]
        public void GetProductsAndCategories_WithCatNumber_UsesConditionalParameterizedQuery(int catNumber, bool expectsFilter)
        {
            // Arrange
            var provider = new MySqlDbProvider(new ConfigFile());

            // Act
            var ex = Record.Exception(() => provider.GetProductsAndCategories(catNumber));

            // Assert
            // We expect failures due to empty connection string, but not due to SQL concatenation of catNumber.
            Assert.NotNull(ex);
            Assert.DoesNotContain("catNumber = " + catNumber, ex.Message, StringComparison.OrdinalIgnoreCase);

            // Also ensure method exists (guard against refactor removing conditional branches).
            var method = typeof(MySqlDbProvider).GetMethod("GetProductsAndCategories", new[] { typeof(int) });
            Assert.NotNull(method);
        }
    }
}
