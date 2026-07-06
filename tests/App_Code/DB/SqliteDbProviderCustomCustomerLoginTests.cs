using System.IO;
using Xunit;

// Source-level regression test: locks in the security-relevant change introduced by PR 3945.
// This test reads the patched source file and asserts the email lookup in CustomCustomerLogin
// uses a parameter placeholder and binds it, instead of string concatenation.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderCustomCustomerLoginTests
    {
        [Fact]
        public void CustomCustomerLogin_EmailQuery_IsParameterized_AndBindsEmail()
        {
            // Arrange
            var path = Path.Combine("WebGoat", "App_Code", "DB", "SqliteDbProvider.cs");
            var code = File.ReadAllText(path);

            // Act/Assert
            Assert.Contains("select * from CustomerLogin where email = @Email;", code);
            Assert.Contains("Parameters.AddWithValue(\"@Email\", email", code);
            Assert.Contains("new SqliteDataAdapter(cmd)", code);

            // Previously vulnerable pattern (string concatenation)
            Assert.DoesNotContain("where email = '\" + email + \"'", code);
        }
    }
}
