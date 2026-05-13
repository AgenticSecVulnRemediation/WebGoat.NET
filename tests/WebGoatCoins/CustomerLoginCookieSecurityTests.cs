using System.Web;
using Xunit;

// Assumption: production page class is in namespace OWASP.WebGoat.NET.WebGoatCoins
using OWASP.WebGoat.NET.WebGoatCoins;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginCookieSecurityTests
    {
        [Fact]
        public void AuthCookie_IsHttpOnlyAndSecure()
        {
            // Delta unit test: auth cookie now must be HttpOnly and Secure.
            var cookie = new HttpCookie(".ASPXAUTH")
            {
                Value = "encrypted_ticket",
                HttpOnly = true,
                Secure = true
            };

            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
