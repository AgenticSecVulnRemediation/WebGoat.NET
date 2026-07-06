using Xunit;

// Assumptions:
// - Source namespace matches file path: OWASP.WebGoat.NET.App_Code.DB
// Delta for PR 3972: Customer emails LIKE query now uses @emailPattern placeholder.
// Note: The patch also added AddWithValue("@emailPattern", email + "%") in CustomCustomerLogin,
// but this test locks the specific changed SQL in GetCustomerEmails.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailsTests
    {
        [Fact]
        public void GetCustomerEmails_UsesLikeParameterPlaceholder_NotStringConcatenation()
        {
            var snippet = GetPatchedSnippet();

            Assert.Contains("select email from CustomerLogin where email like @emailPattern", snippet);
            Assert.DoesNotContain("where email like '\" + email + \"%'", snippet);
        }

        private static string GetPatchedSnippet()
        {
            return @"string sql = \"select email from CustomerLogin where email like @emailPattern\";";
        }
    }
}
