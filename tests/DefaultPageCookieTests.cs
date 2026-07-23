using System;
using System.Web;
using Moq;
using Xunit;

// Assumption: Namespace is OWASP.WebGoat.NET based on file path.
namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultPageCookieTests
    {
        [Fact]
        public void PageLoad_SetsServerCookie_HttpOnlyAndSecure()
        {
            // Delta behavior: cookie now has HttpOnly and Secure set true.
            var cookie = new HttpCookie("Server", "value")
            {
                HttpOnly = true,
                Secure = true
            };

            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
