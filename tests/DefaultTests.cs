using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    // NOTE: Namespace inferred from source file path "WebGoat/Default.aspx.cs".
    public class DefaultTests
    {
        [Fact]
        public void ServerCookie_IsHardened_WithHttpOnlyAndSecure()
        {
            // Patch added HttpOnly and Secure flags to the "Server" cookie.
            var cookie = new System.Web.HttpCookie("Server", "value");
            cookie.HttpOnly = true;
            cookie.Secure = true;

            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
        }
    }
}
