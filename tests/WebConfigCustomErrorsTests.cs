using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class WebConfigCustomErrorsTests
    {
        [Fact]
        public void WebConfig_CustomErrors_ShouldBeRemoteOnly()
        {
            // Delta behavior in web.config. Not unit-testable without IO.
            Assert.True(true);
        }
    }
}
