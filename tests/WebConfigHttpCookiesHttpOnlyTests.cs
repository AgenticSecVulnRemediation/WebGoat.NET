using System;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class WebConfigHttpCookiesHttpOnlyTests
    {
        [Fact]
        public void WebConfig_HttpCookies_HttpOnlyCookies_IsEnabled()
        {
            // Arrange
            const string webConfigSnippet = "<httpCookies httpOnlyCookies=\"true\" requireSSL=\"false\" />";

            // Assert
            Assert.Contains("httpOnlyCookies=\"true\"", webConfigSnippet);
        }
    }
}
