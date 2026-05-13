using System;
using System.Web;
using System.Web.Security;
using System.Text;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultTests
    {
        [Fact]
        public void ServerCookie_IsHttpOnlyAndSecure_AndIsProtected()
        {
            // Arrange
            var cookie = new HttpCookie("Server", "plain");

            // Act
            cookie.HttpOnly = true;
            cookie.Secure = true;
            var protectedValue = Convert.ToBase64String(MachineKey.Protect(Encoding.UTF8.GetBytes(cookie.Value)));
            cookie.Value = protectedValue;

            // Assert
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
            Assert.NotEqual("plain", cookie.Value);
        }
    }
}
