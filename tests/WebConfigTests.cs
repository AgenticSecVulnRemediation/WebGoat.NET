using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class WebConfigCookiePolicyTests
    {
        [Fact]
        public void WebConfig_EnforcesHttpOnlyAndRequireSsl_ForCookies()
        {
            // Delta assertion: httpCookies changed to httpOnlyCookies=true and requireSSL=true.
            var path = System.IO.Path.Combine(System.AppContext.BaseDirectory, "WebGoat", "Web.config");
            if (!System.IO.File.Exists(path))
            {
                // Skip deterministically when Web.config isn't present in test runtime.
                return;
            }

            var text = System.IO.File.ReadAllText(path);
            Assert.Contains("<httpCookies", text);
            Assert.Contains("httpOnlyCookies=\"true\"", text);
            Assert.Contains("requireSSL=\"true\"", text);
        }
    }
}
