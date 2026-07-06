using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class WebConfigHttpCookiesHardeningTests
    {
        [Fact]
        public void WebConfig_HttpCookies_HttpOnlyAndRequireSsl_AreEnabled()
        {
            // Delta regression: httpCookies httpOnlyCookies/requireSSL set to true.
            var xml = System.IO.File.ReadAllText("WebGoat/Web.config");

            Assert.Contains("<httpCookies httpOnlyCookies=\"true\" requireSSL=\"true\"", xml);
            Assert.DoesNotContain("<httpCookies httpOnlyCookies=\"false\" requireSSL=\"false\"", xml);
        }
    }
}
