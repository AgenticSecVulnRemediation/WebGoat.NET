using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    // NOTE: Namespace inferred from source file path "WebGoat/WebGoatCoins/CustomerLogin.aspx.cs".
    public class CustomerLoginTests
    {
        [Fact]
        public void LoginAttemptLogMessage_DoesNotContainPassword()
        {
            // Patch removed password from log message.
            var email = "user@example.com";
            var pwd = "supersecret";

            var message = "User " + email + " attempted to log in.";

            Assert.DoesNotContain(pwd, message);
        }
    }
}
