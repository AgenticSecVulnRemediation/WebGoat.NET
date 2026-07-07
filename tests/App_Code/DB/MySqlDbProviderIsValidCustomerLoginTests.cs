using Xunit;

// Assumption: production code namespace as per file.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderIsValidCustomerLoginTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParameterizedQuery_ForEmailAndPassword()
        {
            // Arrange
            // Delta test for PR #4031: IsValidCustomerLogin moved from string concatenation to @Email/@Password parameters.
            var asm = typeof(MySqlDbProvider).Assembly;
            var bytes = System.IO.File.ReadAllBytes(asm.Location);
            var text = System.Text.Encoding.UTF8.GetString(bytes);

            // Act / Assert
            Assert.Contains("where email = @Email and password = @Password", text);
            Assert.Contains("AddWithValue(\"@Email\"", text);
            Assert.Contains("AddWithValue(\"@Password\"", text);
            Assert.DoesNotContain("where email = '\" + email", text);
        }
    }
}
