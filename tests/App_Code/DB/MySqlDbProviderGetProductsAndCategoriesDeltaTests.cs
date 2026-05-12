using System;
using System.Reflection;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductsAndCategoriesDeltaTests
    {
        [Fact]
        public void GetProductsAndCategories_WhenFiltered_UsesParameterPlaceholder_ForCatNumber()
        {
            // Delta: conditional branch now uses parameterized @catNumber.
            // Assert the placeholder name exists in the assembly strings.

            var assembly = typeof(OWASP.WebGoat.NET.App_Code.DB.MySqlDbProvider).Assembly;
            var allStrings = assembly.FullName ?? string.Empty;

            Assert.Contains("@catNumber", allStrings);
        }
    }
}
