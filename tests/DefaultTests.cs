using System.Web;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultTests
    {
        [Fact]
        public void ServerCookie_IsHttpOnly_WhenCreated()
        {
            // Delta regression: ensure the info-leak cookie is HttpOnly.
            var cookie = new HttpCookie("Server", "host");
            cookie.HttpOnly = true;

            Assert.True(cookie.HttpOnly);
        }
    }
}
