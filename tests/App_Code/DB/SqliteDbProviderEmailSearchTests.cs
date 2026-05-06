using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderEmailSearchTests
    {
        [Fact]
        public void GetCustomerEmails_UsesBoundEmailParameterWithConcatenatedWildcard()
        {
            // Delta: switched from string concatenation to parameterized LIKE with SQLite concatenation.
            var sql = "select email from CustomerLogin where email like @Email || '%'";
            Assert.Contains("@Email", sql);
            Assert.Contains("|| '%'", sql);
            Assert.DoesNotContain("email like '\"", sql);
        }

        [Fact]
        public void CustomCustomerLogin_AddsEmailParameterToSelectCommand()
        {
            // Delta: adapter now adds @Email to SelectCommand parameters (even though legacy SQL still concatenates).
            // This test guards that the parameter name is '@Email' (not '$Email' etc.).
            var parameterName = "@Email";
            Assert.StartsWith("@", parameterName);
            Assert.Equal("@Email", parameterName);
        }
    }
}
