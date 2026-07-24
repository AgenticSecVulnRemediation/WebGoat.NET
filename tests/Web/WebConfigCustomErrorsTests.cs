using Xunit;
using Moq;
using System;
using System.Xml.Linq;

namespace OWASP.WebGoat.NET.Tests
{
    public class WebConfigCustomErrorsTests
    {
        [Fact]
        public void WebConfig_CustomErrors_IsNotOff()
        {
            // Arrange
            var xml = @"<?xml version=\"1.0\"?><configuration><system.web><customErrors mode=\"RemoteOnly\" /></system.web></configuration>";
            var doc = XDocument.Parse(xml);

            // Act
            var mode = doc.Root!
                .Element("system.web")!
                .Element("customErrors")!
                .Attribute("mode")!
                .Value;

            // Assert
            Assert.NotEqual("Off", mode);
            Assert.Equal("RemoteOnly", mode);
        }
    }
}
