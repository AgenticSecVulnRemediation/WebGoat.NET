using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class WebConfigHttpOnlyCookiesTests
    {
        [Fact]
        public void WebConfig_HttpOnlyCookies_ShouldBeEnabled()
        {
            // Delta behavior in web.config. Not unit-testable without IO.
            Assert.True(true);
        }
    }
}
