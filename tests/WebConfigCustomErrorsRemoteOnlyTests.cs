using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class WebConfigCustomErrorsRemoteOnlyTests
    {
        [Fact]
        public void WebConfig_CustomErrors_IsRemoteOnly_WithRedirect()
        {
            // Delta regression: customErrors mode changed from Off to RemoteOnly + defaultRedirect.
            var xml = System.IO.File.ReadAllText("WebGoat/Web.config");

            Assert.Contains("<customErrors mode=\"RemoteOnly\" defaultRedirect=\"~/ErrorPage.aspx\"", xml);
            Assert.DoesNotContain("<customErrors mode=\"Off\"", xml);
        }
    }
}
