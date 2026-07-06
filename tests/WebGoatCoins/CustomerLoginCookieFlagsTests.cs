using System;
using System.IO;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginCookieFlagsTests
    {
        private static string FindRepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                if (Directory.Exists(Path.Combine(dir.FullName, "WebGoat")) && File.Exists(Path.Combine(dir.FullName, "WebGoat", "WebGoatCoins", "CustomerLogin.aspx.cs")))
                    return dir.FullName;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Could not locate repo root containing 'WebGoat/WebGoatCoins/CustomerLogin.aspx.cs'.");
        }

        [Fact]
        public void CustomerLogin_AuthCookie_IsHttpOnlyAndSecure()
        {
            // Delta assertion: auth cookie is now hardened with HttpOnly + Secure.
            var root = FindRepoRoot();
            var file = Path.Combine(root, "WebGoat", "WebGoatCoins", "CustomerLogin.aspx.cs");
            var text = File.ReadAllText(file);

            Assert.Contains("HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName", text);
            Assert.Contains("cookie.HttpOnly = true", text);
            Assert.Contains("cookie.Secure = true", text);
        }
    }
}
