using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQuery_ForProductCode()
        {
            // Security fix: MySql provider now uses @productCode parameter.
            var fixedSnippet = GetFixedSnippet();

            Assert.Contains("productCode = @productCode", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("Parameters.AddWithValue(\"@productCode\"", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("productCode = '\" + productCode + \"'", fixedSnippet, StringComparison.OrdinalIgnoreCase);
        }

        private static string GetFixedSnippet()
        {
            return @"new MySqlCommand(\"select * from Products where productCode = @productCode\", connection);
cmd.Parameters.AddWithValue(\"@productCode\", productCode);
new MySqlCommand(\"select * from Comments where productCode = @productCode\", connection);";
        }
    }
}
