using System;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultPageCookieHardeningTests
    {
        [Fact]
        public void DefaultPage_SetsServerCookieToHttpOnlyAndSecure_InSource()
        {
            // Delta test for PR: cookie hardening (HttpOnly + Secure) added to Default.aspx.cs
            // Source-level regression: ensure the new flags are present in the file.

            var path = System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "WebGoat", "Default.aspx.cs");
            if (!System.IO.File.Exists(path))
            {
                // If repository layout differs in test execution, fail with a clear message.
                throw new InvalidOperationException($"Expected source file not found at {path}");
            }

            var text = System.IO.File.ReadAllText(path);
            Assert.Contains("cookie.HttpOnly = true", text);
            Assert.Contains("cookie.Secure = true", text);
        }
    }
}
