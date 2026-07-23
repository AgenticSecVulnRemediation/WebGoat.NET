using System.IO;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class WebConfigTests_HttpOnlyCookies
    {
        [Fact]
        public void WebConfig_HttpCookies_HttpOnlyCookies_IsTrue()
        {
            // Arrange
            // Patch change: <httpCookies httpOnlyCookies="true" ... />
            var path = Path.Combine("WebGoat", "Web.config");

            // Act
            var xml = File.ReadAllText(path);

            // Assert
            Assert.Contains("<httpCookies", xml);
            Assert.Contains("httpOnlyCookies=\"true\"", xml);
        }
    }
}
