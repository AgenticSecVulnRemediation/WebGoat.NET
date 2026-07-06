using System;
using Xunit;

namespace WebGoat.Tests
{
    public class WebConfigHttpCookiesHardeningTests
    {
        [Fact]
        public void WebConfig_HttpCookies_HttpOnlyAndRequireSsl_AreEnabled()
        {
            // Delta test for PR #3934: <httpCookies httpOnlyCookies="true" requireSSL="true" />
            // This prevents client-side script access and enforces TLS for cookie transport.

            var configPath = System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "WebGoat", "Web.config");
            var text = System.IO.File.ReadAllText(configPath);

            Assert.Contains("<httpCookies httpOnlyCookies=\"true\" requireSSL=\"true\"", text);
            Assert.DoesNotContain("<httpCookies httpOnlyCookies=\"false\" requireSSL=\"false\"", text);
        }
    }
}
