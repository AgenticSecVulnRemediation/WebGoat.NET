using Xunit;

using OWASP.WebGoat.NET.WebGoatCoins;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginCookieHardeningTests
    {
        [Fact]
        public void ButtonLogOnClick_SetsAuthCookieHttpOnly()
        {
            // Arrange/Assert
            // Delta behavior: ensure HttpOnly is set on Forms auth cookie.
            Assert.Contains("cookie.HttpOnly = true", System.IO.File.ReadAllText(typeof(CustomerLogin).Module.FullyQualifiedName));
        }
    }
}
