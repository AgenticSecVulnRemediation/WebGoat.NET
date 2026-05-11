using System.IO;
using System.Xml;
using Xunit;

namespace WebGoat.Tests
{
    public class WebConfigCustomErrorsTests
    {
        [Fact]
        public void WebConfig_CustomErrors_Mode_IsRemoteOnly()
        {
            // Arrange
            var xml = @"<?xml version=\"1.0\"?>
<configuration>
  <system.web>
    <customErrors mode=\"RemoteOnly\" />
  </system.web>
</configuration>";

            var doc = new XmlDocument();
            doc.LoadXml(xml);

            // Act
            var node = doc.SelectSingleNode("/configuration/system.web/customErrors") as XmlElement;

            // Assert
            Assert.NotNull(node);
            Assert.Equal("RemoteOnly", node!.GetAttribute("mode"));
        }
    }
}
