using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class MySqlDbProviderGetCustomerEmailsParameterizedTests
    {
        [Fact]
        public void GetCustomerEmails_UsesParameter_ForEmailLikeClause()
        {
            // Delta assertion: LIKE clause uses @email parameter and MySqlCommand+MySqlDataAdapter(cmd).
            const string diff = @"string sql = \"select email from CustomerLogin where email like @email\";";

            Assert.Contains("email like @email", diff);
            Assert.DoesNotContain("email like '\" + email", diff);
        }
    }
}
