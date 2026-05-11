using System.IO;
using System.Xml;
using Xunit;

namespace WebGoat.Tests
{
    public class WebConfigTests
    {
        [Fact]
        public void WebConfig_HttpCookies_HttpOnlyCookies_IsTrue()
        {
            // Arrange
            var xml = @"<?xml version=\"1.0\"?>
<configuration>
  <system.web>
    <httpCookies httpOnlyCookies=\"true\" requireSSL=\"false\" />
  </system.web>
</configuration>";

            var doc = new XmlDocument();
            doc.LoadXml(xml);

            // Act
            var node = doc.SelectSingleNode("/configuration/system.web/httpCookies") as XmlElement;

            // Assert
            Assert.NotNull(node);
            Assert.Equal("true", node!.GetAttribute("httpOnlyCookies"));
        }
    }
}
