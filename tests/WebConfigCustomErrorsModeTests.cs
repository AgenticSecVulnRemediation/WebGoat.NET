using System;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class WebConfigCustomErrorsModeTests
    {
        [Fact]
        public void WebConfig_CustomErrors_IsNotOff()
        {
            // Delta regression test: customErrors mode changed from Off to RemoteOnly.
            var configPath = System.IO.Path.Combine(AppContext.BaseDirectory, "Web.config");
            if (!System.IO.File.Exists(configPath))
            {
                configPath = "WebGoat/Web.config";
            }

            var xml = System.IO.File.ReadAllText(configPath);
            Assert.Contains("<customErrors mode=\"RemoteOnly\"", xml);
            Assert.DoesNotContain("<customErrors mode=\"Off\"", xml);
        }
    }
}
