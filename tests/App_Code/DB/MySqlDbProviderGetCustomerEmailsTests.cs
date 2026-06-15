using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailsTests
    {
        [Fact]
        public void GetCustomerEmails_UsesLikeParameterAndWildcardConcatenation()
        {
            // Regression test for SQL injection fix: LIKE uses parameter with suffix wildcard.
            var path = System.IO.Path.Combine("WebGoat", "App_Code", "DB", "MySqlDbProvider.cs");
            var content = System.IO.File.ReadAllText(path);

            Assert.Contains("WHERE email LIKE @Email", content);
            Assert.Contains("AddWithValue(\"@Email\", email + \"%\")", content);
            Assert.DoesNotContain("email like '\" + email + \"%\"", content);
        }
    }
}
