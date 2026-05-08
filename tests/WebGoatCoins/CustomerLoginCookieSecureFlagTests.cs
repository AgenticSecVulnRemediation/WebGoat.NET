using System;
using System.Text;
using Xunit;

using OWASP.WebGoat.NET.WebGoatCoins;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginCookieSecureFlagTests
    {
        [Fact]
        public void ButtonLogOn_SetsHttpOnlyAndSecureOnAuthCookie()
        {
            // Delta regression test: added HttpOnly=true and Secure=true on FormsAuthentication cookie.
            var asmText = Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(typeof(CustomerLogin).Assembly.Location));

            Assert.Contains("FormsCookieName", asmText);
            Assert.Contains("cookie.HttpOnly = true", asmText);
            Assert.Contains("cookie.Secure = true", asmText);
        }
    }
}
