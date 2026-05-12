// Assumptions:
// - Namespace in the source file is OWASP.WebGoat.NET.WebGoatCoins
// - This delta test validates that the authentication cookie is hardened by setting HttpOnly and Secure.
// - We validate deterministically by searching for the introduced statements in the compiled assembly.

using System.Reflection;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginCookieHardeningDeltaTests
    {
        [Fact]
        public void CustomerLogin_ButtonLogOn_SetsHttpOnlyAndSecureOnAuthCookie()
        {
            // Arrange
            var asm = typeof(OWASP.WebGoat.NET.WebGoatCoins.CustomerLogin).Assembly;

            // Act
            var allStrings = GetAllUserStrings(asm);

            // Assert (delta)
            Assert.Contains("cookie.HttpOnly = true", allStrings);
            Assert.Contains("cookie.Secure = true", allStrings);
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
