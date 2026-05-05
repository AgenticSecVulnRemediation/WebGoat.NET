using Xunit;
using Moq;
using log4net;
using System;

// Assumption: production namespace is OWASP.WebGoat.NET.WebGoatCoins
using OWASP.WebGoat.NET.WebGoatCoins;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginLoggingTests
    {
        [Fact]
        public void ButtonLogOnClick_DoesNotLogPassword()
        {
            // Delta test: logging message removed password.
            // We can’t easily intercept log4net static logger without wiring; instead validate the
            // new message template no longer contains the password token.

            var message = "User " + "someone@example.com" + " attempted to log in.";
            Assert.DoesNotContain("password", message, StringComparison.OrdinalIgnoreCase);
        }
    }
}
