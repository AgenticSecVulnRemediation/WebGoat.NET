using System;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLogin_AuthCookieFlags_Tests
    {
        [Fact]
        public void CustomerLogin_SetsAuthCookie_SecureAndHttpOnly()
        {
            // Arrange
            // Guard the security fix: cookie flags were added.
            var source = System.IO.File.ReadAllText("WebGoat/WebGoatCoins/CustomerLogin.aspx.cs");

            // Assert
            Assert.Contains("cookie.Secure = true", source);
            Assert.Contains("cookie.HttpOnly = true", source);
        }
    }
}
