using System;
using System.Reflection;
using System.Web;
using Xunit;

// Assumption: code-behind class is in namespace OWASP.WebGoat.NET and named HeaderInjection.

namespace OWASP.WebGoat.NET.Tests
{
    public class HeaderInjectionTests
    {
        [Fact]
        public void HeaderInjectionSource_WhenSettingUserAddedCookie_SetsHttpOnlyTrue()
        {
            // Arrange
            // We assert on code to avoid needing ASP.NET runtime HttpContext pipeline in unit tests.
            var sourcePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "WebGoat", "Content", "HeaderInjection.aspx.cs");
            if (!File.Exists(sourcePath))
            {
                // Fallback for alternative test execution working directory.
                sourcePath = Path.Combine(Directory.GetCurrentDirectory(), "WebGoat", "Content", "HeaderInjection.aspx.cs");
            }

            // Act
            string content = File.ReadAllText(sourcePath);

            // Assert
            Assert.Contains("new HttpCookie(\"UserAddedCookie\")", content);
            Assert.Contains("cookie.HttpOnly = true", content);
        }
    }
}
