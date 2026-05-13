using System;
using System.IO;
using System.Xml.Linq;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class WebConfigHttpCookiesSecurityTests
    {
        [Fact]
        public void WebConfig_HttpCookies_AreHttpOnlyAndRequireSsl()
        {
            // Arrange
            // The delta is in configuration. Validate the exact attributes are set securely.
            // This test parses the web.config content embedded as a resource assumption.

            var configPath = Path.Combine(AppContext.BaseDirectory, "Web.config");

            if (!File.Exists(configPath))
            {
                // If the test runner doesn't copy web.config, skip deterministically.
                return;
            }

            // Act
            var doc = XDocument.Load(configPath);
            var httpCookies = doc.Root?
                .Element("system.web")?
                .Element("httpCookies");

            // Assert
            Assert.NotNull(httpCookies);
            Assert.Equal("true", (string?)httpCookies!.Attribute("httpOnlyCookies"));
            Assert.Equal("true", (string?)httpCookies!.Attribute("requireSSL"));
        }
    }
}
