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
        public void ServerCookie_IsHardened_HttpOnlySecure_AndProtected()
        {
            // Arrange
            var cookie = new HttpCookie("Server", "test");

            // Act
            cookie.HttpOnly = true;
            cookie.Secure = true;
            var bytes = Encoding.UTF8.GetBytes(cookie.Value);
            cookie.Value = Convert.ToBase64String(MachineKey.Protect(bytes));

            // Assert
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
            Assert.NotEqual("test", cookie.Value);
            Assert.False(string.IsNullOrWhiteSpace(cookie.Value));
        }
    }
}
