using System;
using System.IO;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class WebConfigCustomErrorsTests
    {
        [Fact]
        public void WebConfig_CustomErrors_RemoteOnly()
        {
            // Delta behavior: customErrors mode changed to RemoteOnly.
            var path = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "WebGoat", "Web.config");
            var content = File.ReadAllText(path);
            Assert.Contains("<customErrors mode=\"RemoteOnly\"", content);
        }
    }
}
