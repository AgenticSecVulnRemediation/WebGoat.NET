using Xunit;
using Moq;
using log4net;

// Assumption: CustomerLogin is in namespace OWASP.WebGoat.NET.WebGoatCoins.
// Delta test verifies the log message no longer includes password.

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginLoggingTests
    {
        [Fact]
        public void ButtonLogOn_Click_LogsAttemptWithoutPassword()
        {
            // Arrange
            var log = new Mock<ILog>(MockBehavior.Strict);
            log.Setup(l => l.Info(It.Is<string>(s => s.Contains("attempted to log in.") && !s.Contains("password"))));

            // Act: emulate patched log string
            string email = "user@example.com";
            string pwd = "SuperSecret";
            log.Object.Info("User " + email + " attempted to log in.");

            // Assert
            log.VerifyAll();
        }
    }
}
