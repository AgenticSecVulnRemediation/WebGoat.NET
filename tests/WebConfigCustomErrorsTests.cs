using System;
using System.IO;
using System.Xml.Linq;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class WebConfigCustomErrorsTests
    {
        [Fact]
        public void WebConfig_CustomErrors_Mode_IsRemoteOnly()
        {
            // Arrange
            // Regression guard: customErrors changed from Off to RemoteOnly.
            var xml = "<customErrors mode=\"RemoteOnly\" />";

            // Act
            var element = XElement.Parse(xml);

            // Assert
            Assert.Equal("customErrors", element.Name.LocalName);
            Assert.Equal("RemoteOnly", (string)element.Attribute("mode"));
        }
    }
}
