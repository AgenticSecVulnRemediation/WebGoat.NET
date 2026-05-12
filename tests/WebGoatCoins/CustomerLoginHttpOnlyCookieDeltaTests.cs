// Assumptions:
// - Namespace in the source file is OWASP.WebGoat.NET.WebGoatCoins
// - This delta test validates that the authentication cookie is marked HttpOnly.
// - Deterministic verification by scanning compiled assembly strings.

using System.Reflection;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginHttpOnlyCookieDeltaTests
    {
        [Fact]
        public void CustomerLogin_ButtonLogOn_SetsHttpOnlyOnAuthCookie()
        {
            var asm = typeof(OWASP.WebGoat.NET.WebGoatCoins.CustomerLogin).Assembly;
            var allStrings = GetAllUserStrings(asm);

            Assert.Contains("cookie.HttpOnly = true", allStrings);
        }

        private static string GetAllUserStrings(Assembly asm)
        {
            var location = asm.Location;
            if (string.IsNullOrWhiteSpace(location) || !System.IO.File.Exists(location))
            {
                return string.Empty;
            }

            var bytes = System.IO.File.ReadAllBytes(location);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
    }
}
