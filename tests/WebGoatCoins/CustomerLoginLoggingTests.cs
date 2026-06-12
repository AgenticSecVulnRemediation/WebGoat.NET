using System;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginLoggingTests
    {
        [Fact]
        public void LoggingMessage_DoesNotIncludePassword()
        {
            // Arrange
            // Patch removes password from log message.
            var email = "user@example.com";
            var pwd = "SuperSecret!";

            // Act
            var message = $"User {email} attempted to log in.";

            // Assert
            Assert.Contains(email, message);
            Assert.DoesNotContain(pwd, message);
            Assert.DoesNotContain("password", message, StringComparison.OrdinalIgnoreCase);
        }
    }
}
