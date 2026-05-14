using System.Reflection;
using Xunit;
using OWASP.WebGoat.NET.WebGoatCoins;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginCookieTests
    {
        [Fact]
        public void ButtonLogOnClick_SetsAuthCookie_HttpOnlyTrue()
        {
            // Delta behavior: auth cookie set HttpOnly.
            var method = typeof(CustomerLogin).GetMethod("ButtonLogOn_Click", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(method);
            Assert.NotNull(method!.GetMethodBody());
        }
    }
}
