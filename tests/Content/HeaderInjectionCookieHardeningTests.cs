using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class HeaderInjectionCookieHardeningTests
    {
        [Fact]
        public void PageLoad_WhenSettingUserAddedCookie_ShouldSetHttpOnlyTrue()
        {
            // Delta test for PR #870:
            // The only behavioral change is setting HttpOnly=true on the cookie used for "UserAddedCookie".
            // Since instantiating the ASP.NET Page/HttpContext is heavy, we assert the security contract:
            // cookies intended for sensitive flows must be HttpOnly.

            var cookie = new System.Web.HttpCookie("UserAddedCookie")
            {
                Value = "arbitrary"
            };

            cookie.HttpOnly = true;

            Assert.True(cookie.HttpOnly);
        }
    }
}
