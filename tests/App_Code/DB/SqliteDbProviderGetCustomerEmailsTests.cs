using System.IO;
using Xunit;

// Source-level regression test for PR 3972: GetCustomerEmails uses LIKE parameter placeholder.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailsTests
    {
        [Fact]
        public void GetCustomerEmails_UsesLikeParameterPlaceholder()
        {
            var path = Path.Combine("WebGoat", "App_Code", "DB", "SqliteDbProvider.cs");
            var code = File.ReadAllText(path);

            Assert.Contains("select email from CustomerLogin where email like @emailPattern", code);
            Assert.DoesNotContain("where email like '\" + email + \"%'", code);
        }
    }
}
