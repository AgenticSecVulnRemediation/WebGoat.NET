using System;
using Xunit;

namespace WebGoat.Config.Tests
{
    public class WebConfig_CustomErrorsTests
    {
        [Fact]
        public void WebConfig_CustomErrors_IsRemoteOnly()
        {
            // Delta test for PR #641: ensure customErrors mode is RemoteOnly (not Off)
            var xml = System.IO.File.ReadAllText("WebGoat/Web.config");
            Assert.Contains("<customErrors mode=\"RemoteOnly\"", xml);
            Assert.DoesNotContain("<customErrors mode=\"Off\"", xml);
        }
    }
}
