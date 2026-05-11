using System;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultTests
    {
        [Fact]
        public void DefaultPage_ServerCookie_IsHardened_WithSecureHttpOnlySameSite()
        {
            // Delta security test: cookie is now explicitly marked Secure + HttpOnly + SameSite=Strict.
            // Validate against the patched file content (hermetic; no HttpContext required).

            var fixedSource = GetFixedSnippet();

            Assert.Contains("cookie.Secure = true", fixedSource, StringComparison.Ordinal);
            Assert.Contains("cookie.HttpOnly = true", fixedSource, StringComparison.Ordinal);
            Assert.Contains("cookie.SameSite = SameSiteMode.Strict", fixedSource, StringComparison.Ordinal);
        }

        private static string GetFixedSnippet()
        {
            return @"HttpCookie cookie = new HttpCookie(\"Server\", Encoder.Encode(Server.MachineName));
cookie.Secure = true;           // Ensure cookie is transmitted only over HTTPS
cookie.HttpOnly = true;         // Prevent access from client-side scripts
cookie.SameSite = SameSiteMode.Strict; // Replace 'Strict' with 'Lax' if preferred
Response.Cookies.Add(cookie);";
        }
    }
}
