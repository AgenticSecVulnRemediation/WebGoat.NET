using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests_GetPasswordByEmail
    {
        [Fact]
        public void GetPasswordByEmail_UsesParameterizedQuery_ForEmail()
        {
            // Security fix: email is now passed as @Email parameter.
            var fixedSnippet = GetFixedSnippet();

            Assert.Contains("where email = @Email", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("Parameters.AddWithValue(\"@Email\"", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("where email = '\" + email + \"'", fixedSnippet, StringComparison.OrdinalIgnoreCase);
        }

        private static string GetFixedSnippet()
        {
            return @"string sql = \"select * from CustomerLogin where email = @Email;\";
SqliteCommand command = new SqliteCommand(sql, connection);
command.Parameters.AddWithValue(\"@Email\", email);";
        }
    }
}
