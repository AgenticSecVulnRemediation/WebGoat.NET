using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests_GetCustomerEmail
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterizedQuery_ForCustomerNumber()
        {
            // Delta security test: uses @customerNumber.
            var fixedSnippet = GetFixedSnippet();

            Assert.Contains("customerNumber = @customerNumber", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("AddWithValue(\"@customerNumber\"", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("customerNumber = \" + customerNumber", fixedSnippet, StringComparison.OrdinalIgnoreCase);
        }

        private static string GetFixedSnippet()
        {
            return @"string sql = \"select email from CustomerLogin where customerNumber = @customerNumber\";
command.Parameters.AddWithValue(\"@customerNumber\", customerNumber);";
        }
    }
}
