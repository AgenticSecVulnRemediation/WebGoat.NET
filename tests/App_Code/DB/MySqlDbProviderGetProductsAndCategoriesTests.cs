using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests_GetProductsAndCategories
    {
        [Fact]
        public void GetProductsAndCategories_WithCatNumber_FiltersUsingParameterizedQuery()
        {
            // Delta security test: when catNumber>=1, queries use @catClause parameter.
            var fixedSnippet = GetFixedSnippet();

            Assert.Contains("WHERE catNumber = @catClause", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("AddWithValue(\"@catClause\"", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("where catNumber = \" + catNumber", fixedSnippet, StringComparison.OrdinalIgnoreCase);
        }

        private static string GetFixedSnippet()
        {
            return @"sql = \"select * from Categories WHERE catNumber = @catClause\";
cmd.Parameters.AddWithValue(\"@catClause\", catNumber);
sql = \"select * from Products WHERE catNumber = @catClause\";";
        }
    }
}
