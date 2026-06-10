using System;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class WebConfigCookiePolicyTests
    {
        [Fact]
        public void WebConfig_EnforcesHttpOnlyAndRequireSsl_OnHttpCookiesElement()
        {
            // Arrange
            var configPath = System.IO.Path.Combine(AppContext.BaseDirectory, "Web.config");

            // Act
            var xml = System.IO.File.ReadAllText(configPath);

            // Assert (delta): requireSSL and httpOnlyCookies flipped to true
            Assert.Contains("<httpCookies httpOnlyCookies=\"true\" requireSSL=\"true\"", xml);
            Assert.DoesNotContain("httpOnlyCookies=\"false\"", xml);
            Assert.DoesNotContain("requireSSL=\"false\"", xml);
        }
    }
}
