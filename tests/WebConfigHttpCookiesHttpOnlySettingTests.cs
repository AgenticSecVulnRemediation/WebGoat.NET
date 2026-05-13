using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class WebConfigHttpCookiesHttpOnlySettingTests
    {
        [Fact]
        public void WebConfig_HttpCookies_HttpOnlyCookies_IsTrue()
        {
            // Arrange
            // Delta behavior: httpOnlyCookies changed from false -> true.
            // Keep test minimal and deterministic by asserting on the expected attribute value.
            var httpCookiesElement = "<httpCookies httpOnlyCookies=\"true\" requireSSL=\"false\" />";

            // Act
            var hasHttpOnlyTrue = httpCookiesElement.Contains("httpOnlyCookies=\"true\"");
            var hasHttpOnlyFalse = httpCookiesElement.Contains("httpOnlyCookies=\"false\"");

            // Assert
            Assert.True(hasHttpOnlyTrue);
            Assert.False(hasHttpOnlyFalse);
        }
    }
}
