using System;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class WebConfigHttpOnlyCookiesSettingTests
    {
        [Fact]
        public void WebConfig_HttpOnlyCookies_IsEnabled()
        {
            // Delta regression test: httpOnlyCookies was changed from false to true in Web.config.
            // Direct unit test isn't possible; assert the setting value is present in the config file content.
            var configPath = System.IO.Path.Combine(AppContext.BaseDirectory, "Web.config");
            if (!System.IO.File.Exists(configPath))
            {
                // Fallback: allow running from repo root.
                configPath = "WebGoat/Web.config";
            }

            var xml = System.IO.File.ReadAllText(configPath);
            Assert.Contains("httpCookies", xml);
            Assert.Contains("httpOnlyCookies=\"true\"", xml);
        }
    }
}
