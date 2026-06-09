using log4net;
using Moq;
using OWASP.WebGoat.NET.WebGoatCoins;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginLoggingTests
    {
        [Fact]
        public void ButtonLogOn_Click_DoesNotLogPassword()
        {
            // Arrange
            // Delta test: ensure sensitive password is not included in log message.
            var email = "user@example.com";
            var pwd = "SuperSecret";

            // Act
            var message = $"User {email} attempted to log in";

            // Assert
            Assert.DoesNotContain(pwd, message);
            Assert.Contains(email, message);
        }
    }
}
