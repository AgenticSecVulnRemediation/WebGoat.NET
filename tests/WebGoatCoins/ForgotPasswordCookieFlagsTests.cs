using System;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPassword_CookieFlags_Tests
    {
        [Fact]
        public void ForgotPassword_SetsCookie_HttpOnlyAndSecure()
        {
            // Arrange
            var source = System.IO.File.ReadAllText("WebGoat/WebGoatCoins/ForgotPassword.aspx.cs");

            // Assert
            Assert.Contains("cookie.HttpOnly = true", source);
            Assert.Contains("cookie.Secure = true", source);
        }
    }
}
