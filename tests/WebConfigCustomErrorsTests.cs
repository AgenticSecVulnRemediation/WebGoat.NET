using System.IO;
using Xunit;

namespace WebGoat.Tests
{
    public class WebConfig_CustomErrorsTests
    {
        [Fact]
        public void WebConfig_CustomErrors_IsRemoteOnly()
        {
            // Delta test: customErrors mode changed from Off to RemoteOnly to reduce information disclosure.
            var path = Path.Combine(Directory.GetCurrentDirectory(), "WebGoat", "Web.config");
            Assert.True(File.Exists(path), $"Expected Web.config at {path}");

            var xml = File.ReadAllText(path);
            Assert.Contains("<customErrors mode=\"RemoteOnly\"", xml);
            Assert.DoesNotContain("<customErrors mode=\"Off\"", xml);
        }
    }
}
