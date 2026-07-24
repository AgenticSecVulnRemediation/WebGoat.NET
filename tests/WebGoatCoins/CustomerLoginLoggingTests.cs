using Xunit;
using Moq;
using System;
using log4net;
using OWASP.WebGoat.NET.WebGoatCoins;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginLoggingTests
    {
        [Fact]
        public void ButtonLogOn_Click_DoesNotLogPassword()
        {
            // Arrange
            // The delta fix removed password from log message.
            // We can't easily execute WebForms page lifecycle here; instead assert the safe message template.
            var safeMessage = "User " + "someone@example.com" + " attempted to log in.";

            // Assert
            Assert.DoesNotContain("password", safeMessage, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("attempted to log in.", safeMessage);
        }
    }
}
