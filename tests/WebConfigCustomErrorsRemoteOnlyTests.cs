using System;
using System.Xml.Linq;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class WebConfig_CustomErrorsRemoteOnlyTests
    {
        [Fact]
        public void WebConfig_CustomErrors_IsRemoteOnly()
        {
            // Delta guard for PR #427: customErrors mode changed from Off to RemoteOnly.
            var doc = XDocument.Load(GetWebConfigPath());

            var systemWeb = doc.Root?.Element("system.web");
            Assert.NotNull(systemWeb);

            var customErrors = systemWeb!.Element("customErrors");
            Assert.NotNull(customErrors);

            Assert.Equal("RemoteOnly", (string?)customErrors!.Attribute("mode"));
        }

        private static string GetWebConfigPath()
        {
            var path = System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "WebGoat", "Web.config");
            return System.IO.Path.GetFullPath(path);
        }
    }
}
