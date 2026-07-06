using System;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginLoggingTests
    {
        [Fact]
        public void CustomerLogin_DoesNotLogPassword()
        {
            // Delta test for PR #3941: password value must not be included in log message.
            // We lock this in by asserting against the updated source code text.

            var sourcePath = System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "WebGoat", "WebGoatCoins", "CustomerLogin.aspx.cs");
            var text = System.IO.File.ReadAllText(sourcePath);

            Assert.Contains("attempted to log in\")", text);
            Assert.DoesNotContain("attempted to log in with password", text);
            Assert.DoesNotContain("+ pwd", text);
        }
    }
}
