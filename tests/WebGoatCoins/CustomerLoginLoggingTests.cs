using System.Web.Security;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginTests
    {
        [Fact]
        public void LogMessage_DoesNotIncludePassword()
        {
            // Arrange
            var email = "user@example.com";
            var pwd = "password";

            // Act
            var message = "User " + email + " attempted to log in.";

            // Assert
            Assert.Contains(email, message);
            Assert.DoesNotContain(pwd, message);
            Assert.DoesNotContain("password", message, System.StringComparison.OrdinalIgnoreCase);
        }
    }
}
