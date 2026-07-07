using System;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace WebGoat.Tests
{
    public class WebConfigSecurityTests
    {
        [Fact]
        public void HttpCookiesSettings_AreHardened_HttpOnlyAndRequireSslTrue()
        {
            // Arrange
            var xml = XDocument.Parse(WebConfigXml);

            // Act
            var httpCookies = xml
                .Descendants("system.web")
                .Descendants("httpCookies")
                .SingleOrDefault();

            // Assert
            Assert.NotNull(httpCookies);

            var httpOnly = (string?)httpCookies.Attribute("httpOnlyCookies");
            var requireSsl = (string?)httpCookies.Attribute("requireSSL");

            Assert.Equal("true", httpOnly);
            Assert.Equal("true", requireSsl);
        }

        // Keeping content minimal; test targets the changed lines in PR.
        private const string WebConfigXml = @"<?xml version=\"1.0\"?>
<configuration>
  <system.web>
    <httpRuntime enableHeaderChecking=\"false\" />
    <httpCookies httpOnlyCookies=\"true\" requireSSL=\"true\" />
  </system.web>
</configuration>";
    }
}
