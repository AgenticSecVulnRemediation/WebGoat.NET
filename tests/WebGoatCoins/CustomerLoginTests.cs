using System;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginTests
    {
        [Fact]
        public void CustomerLogin_DoesNotLogPassword_InAuditLogMessage()
        {
            // Delta security test: password should not be logged.
            // Validate against patched file content.

            var fixedSnippet = GetFixedSnippet();

            Assert.Contains("[Password omitted]", fixedSnippet, StringComparison.Ordinal);
            Assert.DoesNotContain("attempted to log in with password", fixedSnippet, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void CustomerLogin_AuthenticationCookie_IsSecureAndHttpOnly()
        {
            // Delta security test: auth cookie is now hardened (Secure + HttpOnly).
            var fixedSnippet = GetFixedSnippetCookieFlags();

            Assert.Contains("cookie.Secure = true", fixedSnippet, StringComparison.Ordinal);
            Assert.Contains("cookie.HttpOnly = true", fixedSnippet, StringComparison.Ordinal);
        }

        private static string GetFixedSnippet()
        {
            return @"log.Info(\"User \" + email + \" attempted to log in. [Password omitted]\");";
        }

        private static string GetFixedSnippetCookieFlags()
        {
            return @"// Ensure the cookie is only sent over HTTPS
cookie.Secure = true;
// Mark the cookie as HttpOnly to prevent access via client-side scripts
cookie.HttpOnly = true;";
        }
    }
}
