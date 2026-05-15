using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class WebConfigCustomErrorsTests
    {
        [Fact]
        public void WebConfig_UsesRemoteOnly_CustomErrorsMode()
        {
            // Delta test: ensure security fix changed customErrors mode from Off to RemoteOnly.
            // Note: unit tests normally don't parse config files, but this repo includes Web.config in source tree.
            var configText = System.IO.File.ReadAllText("WebGoat/Web.config");
            Assert.Contains("<customErrors mode=\"RemoteOnly\"", configText);
            Assert.DoesNotContain("<customErrors mode=\"Off\"", configText);
        }
    }
}
