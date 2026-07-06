using System;
using System.IO;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultPageCookieHardeningTests
    {
        private static string FindRepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                if (Directory.Exists(Path.Combine(dir.FullName, "WebGoat")) && File.Exists(Path.Combine(dir.FullName, "WebGoat", "Default.aspx.cs")))
                    return dir.FullName;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Could not locate repo root containing 'WebGoat/Default.aspx.cs'.");
        }

        [Fact]
        public void DefaultPage_ServerCookie_IsHttpOnlyAndSecure()
        {
            // Delta assertion: the Server info-leak cookie is now hardened with HttpOnly + Secure.
            var root = FindRepoRoot();
            var file = Path.Combine(root, "WebGoat", "Default.aspx.cs");
            var text = File.ReadAllText(file);

            Assert.Contains("HttpCookie cookie = new HttpCookie(\"Server\"", text);
            Assert.Contains("cookie.HttpOnly = true", text);
            Assert.Contains("cookie.Secure = true", text);
        }
    }
}
