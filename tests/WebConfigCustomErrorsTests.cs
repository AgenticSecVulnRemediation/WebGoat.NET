using Xunit;
using System.IO;
using System.Text.RegularExpressions;

namespace OWASP.WebGoat.NET.Tests
{
    public class WebConfigCustomErrorsTests
    {
        [Fact]
        public void WebConfig_CustomErrors_IsNotOff()
        {
            // Delta behavior: customErrors was changed from Off to RemoteOnly.
            var path = Path.Combine("WebGoat", "Web.config");
            var content = File.ReadAllText(path);

            Assert.Matches(new Regex("<customErrors\\s+mode=\"RemoteOnly\"\\s*/>", RegexOptions.IgnoreCase), content);
            Assert.DoesNotMatch(new Regex("<customErrors\\s+mode=\"Off\"", RegexOptions.IgnoreCase), content);
        }
    }
}
