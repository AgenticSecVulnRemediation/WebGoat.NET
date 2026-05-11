using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests_UpdateCustomerPassword
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameterizedUpdate_ForPasswordAndCustomerNumber()
        {
            // Delta security test: update now uses @password and @customerNumber.
            var fixedSnippet = GetFixedSnippet();

            Assert.Contains("set password = @password", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("customerNumber = @customerNumber", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("AddWithValue(\"@password\"", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("AddWithValue(\"@customerNumber\"", fixedSnippet, StringComparison.OrdinalIgnoreCase);

            Assert.DoesNotContain("set password = '\" +", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("where customerNumber = \" +", fixedSnippet, StringComparison.OrdinalIgnoreCase);
        }

        private static string GetFixedSnippet()
        {
            return @"string sql = \"update CustomerLogin set password = @password where customerNumber = @customerNumber\";
command.Parameters.AddWithValue(\"@password\", Encoder.Encode(password));
command.Parameters.AddWithValue(\"@customerNumber\", customerNumber);";
        }
    }
}
