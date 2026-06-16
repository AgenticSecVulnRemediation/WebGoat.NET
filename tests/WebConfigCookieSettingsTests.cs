using System;
using System.IO;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class WebConfigCookieSettingsTests
    {
        [Fact]
        public void WebConfig_HttpCookies_AreHttpOnlyAndRequireSsl()
        {
            // Arrange
            var configPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "WebGoat", "Web.config");
            if (!File.Exists(configPath))
            {
                configPath = Path.Combine(Directory.GetCurrentDirectory(), "WebGoat", "Web.config");
            }

            // Act
            string content = File.ReadAllText(configPath);

            // Assert
            Assert.Contains("<httpCookies", content);
            Assert.Contains("httpOnlyCookies=\"true\"", content);
            Assert.Contains("requireSSL=\"true\"", content);
        }
    }
}
