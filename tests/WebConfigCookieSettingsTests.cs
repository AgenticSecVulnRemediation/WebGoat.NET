using System;
using System.IO;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class WebConfigCookieSettingsTests
    {
        [Fact]
        public void WebConfig_HttpCookies_HttpOnlyCookiesEnabled()
        {
            // Delta behavior: httpCookies httpOnlyCookies="true".
            var path = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "WebGoat", "Web.config");
            var content = File.ReadAllText(path);
            Assert.Contains("<httpCookies httpOnlyCookies=\"true\"", content);
        }
    }
}
