using System;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    // Delta-focused test for PR 402:
    // Web.config sets httpCookies httpOnlyCookies=true and requireSSL=true.
    // Since .config isn't executed in unit tests, we validate the expected config values by parsing.
    public class WebConfigHttpCookiesSettingsTests
    {
        [Fact]
        public void WebConfig_ShouldRequireHttpOnlyCookies_AndSsl()
        {
            // Assumption: tests run from repository root; Web.config at WebGoat/Web.config.
            var path = System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "WebGoat", "Web.config");
            path = System.IO.Path.GetFullPath(path);

            Assert.True(System.IO.File.Exists(path), $"Expected Web.config at {path}");

            var xml = System.IO.File.ReadAllText(path);
            Assert.Contains("<httpCookies", xml, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("httpOnlyCookies=\"true\"", xml, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("requireSSL=\"true\"", xml, StringComparison.OrdinalIgnoreCase);
        }
    }
}
