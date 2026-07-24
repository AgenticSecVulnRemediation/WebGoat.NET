using System;
using System.IO;
using System.Linq;
using Xunit;

namespace WebGoat.Tests
{
    public class WebConfigCookieHardeningTests
    {
        [Fact]
        public void WebConfig_HttpCookies_HttpOnlyAndRequireSsl_AreTrue()
        {
            // Arrange
            // This delta test asserts the specific security regression fixed in PR 4691:
            // httpOnlyCookies and requireSSL must be set to true.
            // NOTE: If the project uses a different runtime path, adjust accordingly.
            var webConfigPath = Path.Combine("WebGoat", "Web.config");

            // Act
            var content = File.ReadAllText(webConfigPath);

            // Assert
            Assert.Contains("<httpCookies", content, StringComparison.OrdinalIgnoreCase);

            // naive attribute checks are fine for a delta test; XML parsing could be added but not required.
            Assert.Contains("httpOnlyCookies=\"true\"", content, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("requireSSL=\"true\"", content, StringComparison.OrdinalIgnoreCase);

            // Ensure the previously insecure defaults are not present
            Assert.DoesNotContain("httpOnlyCookies=\"false\"", content, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("requireSSL=\"false\"", content, StringComparison.OrdinalIgnoreCase);
        }
    }
}
