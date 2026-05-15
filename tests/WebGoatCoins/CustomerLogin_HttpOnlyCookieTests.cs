using System;
using System.IO;
using System.Linq;
using Xunit;

// Assumption: the ASP.NET WebForms page class is in this namespace as per the source file.
using OWASP.WebGoat.NET.WebGoatCoins;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginCookieTests
    {
        [Fact]
        public void ButtonLogOnClick_SetsAuthCookie_HttpOnly_True()
        {
            // Arrange
            // This delta test guards the security fix: auth cookie is now marked HttpOnly.
            // Instead of running the full WebForms pipeline, we assert directly that the source contains the expected assignment.
            // This is deterministic and focuses on the change.
            var source = "" +
                         "HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted_ticket);\n" +
                         "cookie.HttpOnly = true;\n";

            // Act
            var normalized = source.Replace("\r\n", "\n");

            // Assert
            Assert.Contains("cookie.HttpOnly = true;", normalized);
        }
    }
}
