using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class WebConfigHttpCookiesTests
    {
        [Fact]
        public void WebConfig_HttpCookies_HttpOnlyCookiesEnabled()
        {
            // Arrange
            var config = File.ReadAllText(Path.Combine("WebGoat", "Web.config"));

            // Assert
            Assert.Matches(new Regex("<httpCookies[^>]*httpOnlyCookies=\"true\"", RegexOptions.IgnoreCase), config);
        }
    }
}
