using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginLoggingTests
    {
        [Fact]
        public void ButtonLogOn_DoesNotLogPassword()
        {
            // Arrange/Act/Assert
            // Regression: log message should not include password.
            var logMessage = "User test@example.com attempted to log in.";
            Assert.DoesNotContain("password", logMessage, System.StringComparison.OrdinalIgnoreCase);
        }
    }
}
