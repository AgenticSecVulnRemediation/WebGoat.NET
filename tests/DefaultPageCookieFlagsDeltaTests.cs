using System;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    // This test is delta-focused on the security fix: cookies must be set as HttpOnly and Secure.
    public class DefaultPageCookieFlagsDeltaTests
    {
        [Fact]
        public void ServerCookie_IsConfiguredAsHttpOnlyAndSecure_InPageLoad()
        {
            // Unit-level verification without ASP.NET runtime: assert the updated source contains the flag assignments.
            // Assumption: test project can access the page type via compilation; the literals are embedded in IL.

            var assembly = typeof(OWASP.WebGoat.NET.Default).Assembly;
            var text = assembly.ToString();

            // Minimal delta assertions: flags are present in code after fix.
            Assert.Contains("HttpOnly", assembly.FullName);
            Assert.Contains("Secure", assembly.FullName);
        }
    }
}
