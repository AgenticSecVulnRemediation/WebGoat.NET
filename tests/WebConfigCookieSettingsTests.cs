using System.IO;
using System.Xml.Linq;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class WebConfigCookieSettingsTests
    {
        [Fact]
        public void WebConfig_HttpCookies_HttpOnlyCookies_IsTrue()
        {
            // Arrange
            var configPath = Path.Combine("WebGoat", "Web.config");

            // Act
            var xml = XDocument.Load(configPath);
            var httpCookies = xml.Root?
                .Element("system.web")?
                .Element("httpCookies");

            // Assert
            Assert.NotNull(httpCookies);
            Assert.Equal("true", httpCookies!.Attribute("httpOnlyCookies")?.Value);
        }
    }
}
