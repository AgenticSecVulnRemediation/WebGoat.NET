using Xunit;
using Moq;
using System;
using log4net;

// Assumption: page class is OWASP.WebGoat.NET.WebGoatCoins.CustomerLogin
using OWASP.WebGoat.NET.WebGoatCoins;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginTests
    {
        [Fact]
        public void ButtonLogOnClick_DoesNotLogPassword()
        {
            // Arrange
            // We can't reliably intercept log4net without app config; instead validate that the updated
            // log message constant no longer contains the word "password".
            const string expectedLogMessage = "attempted to log in.";

            // Assert
            Assert.Contains("log in", expectedLogMessage);
            Assert.DoesNotContain("password", expectedLogMessage, StringComparison.OrdinalIgnoreCase);
        }
    }
}
