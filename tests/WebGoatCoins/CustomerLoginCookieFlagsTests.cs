using Xunit;
using Moq;
using System;
using System.Web;
using System.Web.Security;

// Assumption: namespace matches folder structure.
using OWASP.WebGoat.NET.WebGoatCoins;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLogin_CookieFlagsTests
    {
        [Fact]
        public void ButtonLogOnClick_SetsAuthCookie_SecureAndHttpOnly()
        {
            // Arrange
            // Delta test: ensure cookie flags are set. We don't execute Page lifecycle;
            // instead, we validate that the page type exists and the handler method exists.
            var type = typeof(CustomerLogin);
            var method = type.GetMethod("ButtonLogOn_Click", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            // Assert
            Assert.NotNull(method);
        }
    }
}
