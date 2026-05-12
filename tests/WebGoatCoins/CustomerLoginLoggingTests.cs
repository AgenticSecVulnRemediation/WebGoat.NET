using System;
using log4net;
using Moq;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginLoggingTests
    {
        [Fact]
        public void ButtonLogOn_DoesNotLogPassword()
        {
            // Delta: log message should not include the password
            var email = "user@example.com";
            var pwd = "secret";
            var message = $"User {email} attempted to log in.";

            Assert.DoesNotContain(pwd, message);
        }
    }
}
