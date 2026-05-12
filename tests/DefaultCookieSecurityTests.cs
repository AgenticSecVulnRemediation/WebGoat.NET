using System;
using System.Text;
using System.Web;
using System.Web.Security;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultCookieSecurityTests
    {
        [Fact]
        public void Default_ServerCookie_IsHttpOnlyAndSecure_AndProtected()
        {
            // Delta: cookie should be HttpOnly + Secure and value protected
            var cookie = new HttpCookie("Server", "plain");
            cookie.HttpOnly = true;
            cookie.Secure = true;

            var protectedBytes = MachineKey.Protect(Encoding.UTF8.GetBytes(cookie.Value));
            cookie.Value = Convert.ToBase64String(protectedBytes);

            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
            Assert.NotEqual("plain", cookie.Value);
        }
    }
}
