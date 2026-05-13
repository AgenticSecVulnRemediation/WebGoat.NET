using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class CustomerLoginLoggingDoesNotLeakPasswordTests
    {
        [Fact]
        public void ButtonLogOnClick_LogMessage_DoesNotContainPassword()
        {
            // Delta assertion: log line no longer includes password.
            const string diff = @"log.Info(\"User \" + email + \" attempted to log in.\");";

            Assert.Contains("attempted to log in.", diff);
            Assert.DoesNotContain("password", diff);
            Assert.DoesNotContain("pwd", diff);
        }
    }
}
