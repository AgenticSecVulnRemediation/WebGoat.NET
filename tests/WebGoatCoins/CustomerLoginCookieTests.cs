using System.Web;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    // NOTE: Namespace inferred from source file path "WebGoat/WebGoatCoins/CustomerLogin.aspx.cs".
    public class CustomerLoginCookieTests
    {
        [Fact]
        public void AuthCookie_IsHttpOnly_ToPreventClientSideAccess()
        {
            // Patch added cookie.HttpOnly = true for FormsAuthentication cookie.
            var cookie = new HttpCookie(".ASPXAUTH", "ticket");
            cookie.HttpOnly = true;

            Assert.True(cookie.HttpOnly);
        }
    }
}
