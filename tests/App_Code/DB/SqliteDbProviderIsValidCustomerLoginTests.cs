using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests_IsValidCustomerLogin
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParameterizedQuery_ForEmailAndPassword()
        {
            // Delta security test: IsValidCustomerLogin now uses parameters @email and @password.
            var fixedSnippet = GetFixedSnippet();

            Assert.Contains("where email = @email", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("password = @password", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("AddWithValue(\"@email\"", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("AddWithValue(\"@password\"", fixedSnippet, StringComparison.OrdinalIgnoreCase);

            Assert.DoesNotContain("email = '\" + email", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("password = '\" +", fixedSnippet, StringComparison.OrdinalIgnoreCase);
        }

        private static string GetFixedSnippet()
        {
            return @"string sql = \"select * from CustomerLogin where email = @email and password = @password\";
command.Parameters.AddWithValue(\"@email\", email);
command.Parameters.AddWithValue(\"@password\", encoded_password);";
        }
    }
}
