using System;
using System.IO;
using System.Xml.Linq;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class WebConfig_CustomErrors_RemoteOnly_Tests
    {
        [Fact]
        public void WebConfig_CustomErrorsMode_IsRemoteOnly()
        {
            // Arrange
            var xml = File.ReadAllText("WebGoat/Web.config");
            var doc = XDocument.Parse(xml);

            // Act
            XNamespace ns = doc.Root!.Name.Namespace;
            var customErrors = doc.Root!.Element("system.web")!.Element("customErrors");

            // Assert
            Assert.NotNull(customErrors);
            Assert.Equal("RemoteOnly", customErrors!.Attribute("mode")!.Value);
        }
    }
}
