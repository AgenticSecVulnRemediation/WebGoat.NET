using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;

// Delta test: verifies Web.config sets httpCookies httpOnlyCookies="true".

namespace WebGoat.Tests
{
    public class WebConfigHttpOnlyCookiesTests
    {
        [Fact]
        public void WebConfig_HttpCookies_HttpOnlyCookies_IsTrue()
        {
            // Arrange
            var path = Path.Combine(AppContext.BaseDirectory, "Web.config");

            // If tests run from different base dir, fall back to repository-relative typical location.
            if (!File.Exists(path))
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), "WebGoat", "Web.config");
            }

            Assert.True(File.Exists(path), $"Web.config not found at {path}");

            // Act
            var doc = XDocument.Load(path);
            XNamespace ns = doc.Root?.Name.Namespace ?? XNamespace.None;
            var httpCookies = doc.Descendants("httpCookies").FirstOrDefault();

            // Assert
            Assert.NotNull(httpCookies);
            Assert.Equal("true", httpCookies!.Attribute("httpOnlyCookies")?.Value);
        }
    }
}
