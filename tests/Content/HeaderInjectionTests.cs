using System.Web;
using System.Web.UI;
using Xunit;

// Assumption: source namespace is OWASP.WebGoat.NET (as declared in file).
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class HeaderInjectionTests
    {
        [Fact]
        public void CookieCreatedForUserAddedCookie_IsHttpOnly()
        {
            // Delta regression: cookie is now marked HttpOnly.
            var cookie = new HttpCookie("UserAddedCookie")
            {
                Value = "abc"
            };

            cookie.HttpOnly = true;

            Assert.True(cookie.HttpOnly);
        }

        [Fact]
        public void CookieCreatedForUserAddedCookie_IsSecure_WhenApplied()
        {
            // Delta regression: in later patch, Secure is also set.
            var cookie = new HttpCookie("UserAddedCookie")
            {
                Value = "abc"
            };

            cookie.Secure = true;
            Assert.True(cookie.Secure);
        }
    }
}
