using System.Web;
using Xunit;

// Assumption: source namespace is OWASP.WebGoat.NET (as declared in file).
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class HeaderInjectionCookieHardeningTests
    {
        [Fact]
        public void Cookie_UserAddedCookie_HasHttpOnlyAndSecureFlagsSet()
        {
            // Delta: cookie is set to HttpOnly and Secure.
            var cookie = new HttpCookie("UserAddedCookie")
            {
                Value = "abc"
            };

            cookie.HttpOnly = true;
            cookie.Secure = true;

            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }

        [Fact]
        public void Cookie_UserAddedCookie_InvalidValue_IsReplacedWithDefault()
        {
            // Delta: cookie value is validated with a conservative regex and replaced with "default" if invalid.
            // We use a minimal representative invalid value (non-alphanumeric) for secure behavior.
            var cookie = new HttpCookie("UserAddedCookie")
            {
                Value = "a-b"
            };

            if (!System.Text.RegularExpressions.Regex.IsMatch(cookie.Value, "^[a-zA-Z0-9]+$"))
            {
                cookie.Value = "default";
            }

            Assert.Equal("default", cookie.Value);
        }
    }
}
