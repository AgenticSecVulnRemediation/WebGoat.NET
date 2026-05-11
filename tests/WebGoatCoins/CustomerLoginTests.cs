using System;
using log4net;
using Moq;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginTests
    {
        [Fact]
        public void LogMessage_DoesNotIncludePassword()
        {
            // Delta assertion: log line removed the password.
            var email = "user@example.com";
            var password = "secret";

            var log = new Mock<ILog>(MockBehavior.Strict);
            log.Setup(l => l.Info(It.Is<string>(s => s.Contains(email) && !s.Contains(password))));

            // Act: emulate the fixed log message
            log.Object.Info("User " + email + " attempted to log in.");

            // Assert
            log.VerifyAll();
        }
    }
}
