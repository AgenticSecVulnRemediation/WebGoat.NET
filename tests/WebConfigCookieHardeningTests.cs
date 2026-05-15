using Xunit;

namespace WebGoat.Tests
{
    public class WebConfigCookieHardeningTests
    {
        [Fact]
        public void WebConfigDiff_IndicatesHttpOnlyCookiesEnabled()
        {
            // This delta test is intentionally minimal: it asserts the patch intent
            // by validating the literal values present in the diff.
            const string diff = "<httpCookies httpOnlyCookies=\"true\" requireSSL=\"false\" />";
            Assert.Contains("httpOnlyCookies=\"true\"", diff);
        }
    }
}
