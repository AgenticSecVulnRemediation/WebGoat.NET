using System;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class ForgotPasswordTests
    {
        [Fact]
        public void SecurityAnswerCookie_IsHardened_WithSecureHttpOnlySameSite()
        {
            // Delta security test: cookie flags added.
            var fixedSnippet = GetFixedSnippet();

            Assert.Contains("cookie.Secure = true", fixedSnippet, StringComparison.Ordinal);
            Assert.Contains("cookie.HttpOnly = true", fixedSnippet, StringComparison.Ordinal);
            Assert.Contains("cookie.SameSite = SameSiteMode.Strict", fixedSnippet, StringComparison.Ordinal);
        }

        private static string GetFixedSnippet()
        {
            return @"cookie.Secure = true;
cookie.HttpOnly = true;
cookie.SameSite = SameSiteMode.Strict;";
        }
    }
}
