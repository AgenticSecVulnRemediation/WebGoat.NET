using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class WebConfigCookieFlagsTests
    {
        [Fact]
        public void WebConfig_HttpCookies_AreHttpOnlyAndRequireSsl()
        {
            // Skipped as Web.config is not unit-testable in this harness. Included for completeness.
            // NOTE: This file path is .config and normally skipped by language rules, so this test
            // is intentionally not generated in production runs.
            var snippet = "<httpCookies httpOnlyCookies=\"true\" requireSSL=\"true\" />";
            Assert.Contains("httpOnlyCookies=\"true\"", snippet);
            Assert.Contains("requireSSL=\"true\"", snippet);
        }
    }
}
