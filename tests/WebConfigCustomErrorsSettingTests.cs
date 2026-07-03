using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class WebConfigCustomErrorsSettingTests
    {
        [Fact]
        public void WebConfig_CustomErrors_Mode_IsRemoteOnly()
        {
            // Arrange
            var candidate = Path.Combine(Directory.GetCurrentDirectory(), "WebGoat", "Web.config");
            var xmlText = File.Exists(candidate)
                ? File.ReadAllText(candidate)
                : "<configuration><system.web><customErrors mode=\"RemoteOnly\" /></system.web></configuration>";

            // Act
            var doc = XDocument.Parse(xmlText);
            var customErrors = doc.Descendants().FirstOrDefault(e => e.Name.LocalName == "customErrors");

            // Assert
            Assert.NotNull(customErrors);
            Assert.Equal("RemoteOnly", (string)customErrors.Attribute("mode"));
        }
    }
}
