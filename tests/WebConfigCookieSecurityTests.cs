using Xunit;
using System.IO;
using System.Text.RegularExpressions;

namespace OWASP.WebGoat.NET.Tests
{
    public class WebConfigCookieSecurityTests
    {
        [Fact]
        public void WebConfig_HttpCookies_HasHttpOnlyAndRequireSslEnabled()
        {
            // Delta behavior: httpOnlyCookies and requireSSL were set to true.
            var path = Path.Combine("WebGoat", "Web.config");
            var content = File.ReadAllText(path);

            Assert.Matches(new Regex("<httpCookies[^>]*httpOnlyCookies=\"true\"[^>]*>", RegexOptions.IgnoreCase), content);
            Assert.Matches(new Regex("<httpCookies[^>]*requireSSL=\"true\"[^>]*>", RegexOptions.IgnoreCase), content);
        }
    }
}
