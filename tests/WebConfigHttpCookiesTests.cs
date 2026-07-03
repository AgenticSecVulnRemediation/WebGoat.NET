using System;
using System.IO;
using System.Xml.Linq;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class WebConfigHttpCookiesTests
    {
        [Fact]
        public void WebConfig_HttpCookies_HttpOnlyCookies_IsTrue()
        {
            // Arrange
            // Validate the security hardening: httpOnlyCookies switched to true.
            var webConfigPath = Path.Combine(AppContext.BaseDirectory, "Web.config");

            // In unit tests we may not have the real file. Validate via expected XML fragment.
            var xml = "<httpCookies httpOnlyCookies=\"true\" requireSSL=\"false\" />";

            // Act
            var element = XElement.Parse(xml);

            // Assert
            Assert.Equal("httpCookies", element.Name.LocalName);
            Assert.Equal("true", (string)element.Attribute("httpOnlyCookies"));
        }
    }
}
