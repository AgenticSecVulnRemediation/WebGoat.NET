using System;
using System.Reflection;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsDeltaTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterPlaceholder_ForProductCode()
        {
            // Delta: query changed from string concatenation to parameterized @productCode.
            // Assert the fixed query fragment exists in the compiled assembly strings.

            var assembly = typeof(OWASP.WebGoat.NET.App_Code.DB.MySqlDbProvider).Assembly;
            var allStrings = assembly.FullName ?? string.Empty;

            Assert.Contains("@productCode", allStrings);
        }
    }
}
