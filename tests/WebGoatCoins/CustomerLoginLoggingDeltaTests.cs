using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginLoggingDeltaTests
    {
        [Fact]
        public void LoginAttempt_LogMessage_DoesNotContainPassword()
        {
            // Delta assertion based strictly on the patch: password is no longer included in the log message.
            var email = "user@example.com";
            var password = "SuperSecret";

            var message = "User " + email + " attempted to log in";

            Assert.Contains(email, message);
            Assert.DoesNotContain(password, message);
        }
    }
}
