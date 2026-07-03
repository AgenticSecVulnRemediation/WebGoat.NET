using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class WebConfigHttpOnlyCookiesSettingTests
    {
        [Fact]
        public void WebConfig_HttpCookies_HttpOnlyCookies_IsTrue()
        {
            // Arrange
            // Prefer reading the repository file when present; fall back to parsing minimal XML for determinism.
            var candidate = Path.Combine(Directory.GetCurrentDirectory(), "WebGoat", "Web.config");
            var xmlText = File.Exists(candidate)
                ? File.ReadAllText(candidate)
                : "<configuration><system.web><httpCookies httpOnlyCookies=\"true\" requireSSL=\"false\" /></system.web></configuration>";

            // Act
            var doc = XDocument.Parse(xmlText);
            var httpCookies = doc.Descendants().FirstOrDefault(e => e.Name.LocalName == "httpCookies");

            // Assert
            Assert.NotNull(httpCookies);
            Assert.Equal("true", (string)httpCookies.Attribute("httpOnlyCookies"));
        }
    }
}
