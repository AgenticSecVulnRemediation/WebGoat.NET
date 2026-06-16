using System;
using System.IO;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultPageCookieTests
    {
        [Fact]
        public void DefaultPageSource_ServerInfoCookie_IsMarkedHttpOnly()
        {
            // Arrange
            var sourcePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "WebGoat", "Default.aspx.cs");
            if (!File.Exists(sourcePath))
            {
                sourcePath = Path.Combine(Directory.GetCurrentDirectory(), "WebGoat", "Default.aspx.cs");
            }

            // Act
            string content = File.ReadAllText(sourcePath);

            // Assert
            Assert.Contains("new HttpCookie(\"Server\"", content);
            Assert.Contains("cookie.HttpOnly = true", content);
        }
    }
}
