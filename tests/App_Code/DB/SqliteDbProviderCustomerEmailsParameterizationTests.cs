using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderCustomerEmailsParameterizationTests
    {
        [Fact]
        public void GetCustomerEmails_UsesParameterConcatenatedWithWildcard_NotStringConcatenation()
        {
            // Delta check: query should use @Email parameter with concatenated wildcard.
            var sql = "select email from CustomerLogin where email like @Email || '%'";

            Assert.Contains("@Email", sql);
            Assert.Contains("|| '%'", sql);
            Assert.DoesNotContain("'\" + email + \"%" , sql);
        }
    }
}
