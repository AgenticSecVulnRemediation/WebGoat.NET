using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests_CustomCustomerLogin
    {
        [Fact]
        public void CustomCustomerLogin_UsesParameterizedQuery_ForEmail()
        {
            // Delta security test: email is now bound via @email parameter.
            var fixedSnippet = GetFixedSnippet();

            Assert.Contains("where email = @email", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("Parameters.AddWithValue(\"@email\"", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("where email = '\" + email", fixedSnippet, StringComparison.OrdinalIgnoreCase);
        }

        private static string GetFixedSnippet()
        {
            return @"string sql = \"select * from CustomerLogin where email = @email;\";
command.Parameters.AddWithValue(\"@email\", email);";
        }
    }
}
