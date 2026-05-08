using System;
using System.Text;
using Xunit;

using OWASP.WebGoat.NET.WebGoatCoins;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginCookieHttpOnlyFlagTests
    {
        [Fact]
        public void ButtonLogOn_SetsHttpOnlyOnAuthCookie()
        {
            // Delta regression test: added HttpOnly=true on FormsAuthentication cookie.
            var asmText = Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(typeof(CustomerLogin).Assembly.Location));

            Assert.Contains("cookie.HttpOnly = true", asmText);
        }
    }
}
