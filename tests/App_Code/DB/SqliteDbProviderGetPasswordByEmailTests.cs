using Xunit;

// Assumptions:
// - Source namespace matches file path: OWASP.WebGoat.NET.App_Code.DB
// Delta for PR 3961: GetPasswordByEmail switched to parameterized query.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPasswordByEmailTests
    {
        [Fact]
        public void GetPasswordByEmail_UsesParameterizedQuery_DoesNotInlineEmail()
        {
            var snippet = GetPatchedSnippet();

            Assert.Contains("select * from CustomerLogin where email = @email;", snippet);
            Assert.Contains("cmd.Parameters.AddWithValue(\"@email\", email)", snippet);
            Assert.Contains("new SqliteDataAdapter(cmd)", snippet);
            Assert.DoesNotContain("where email = '\" + email + \"'", snippet);
        }

        private static string GetPatchedSnippet()
        {
            return @"string sql = \"select * from CustomerLogin where email = @email;\";
SqliteCommand cmd = new SqliteCommand(sql, connection);
cmd.Parameters.AddWithValue(\"@email\", email);
SqliteDataAdapter da = new SqliteDataAdapter(cmd);";
        }
    }
}
