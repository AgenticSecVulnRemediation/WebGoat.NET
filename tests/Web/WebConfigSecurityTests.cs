using Xunit;

namespace WebGoat.Web.Tests
{
    public class WebConfigSecurityTests
    {
        [Fact]
        public void WebConfig_HttpCookies_IsHttpOnlyEnabled()
        {
            // Arrange
            // Note: Using a literal check rather than loading XML via ConfigurationManager to keep the test deterministic.
            // If the project already has config loading helpers, this can be adapted.
            var webConfig = System.IO.File.ReadAllText("WebGoat/Web.config");

            // Act / Assert
            Assert.Contains("<httpCookies", webConfig);
            Assert.Contains("httpOnlyCookies=\"true\"", webConfig);
        }
    }
}
