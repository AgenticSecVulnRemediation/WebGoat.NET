using System;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginCookieFlagsTests
    {
        [Fact]
        public void CustomerLogin_SetsAuthCookieHttpOnlyAndSecure_InSource()
        {
            // Delta test for PR: auth cookie is now marked HttpOnly and Secure.
            // Source-level regression because ASP.NET types are hard to instantiate without web runtime.

            var path = System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "WebGoat", "WebGoatCoins", "CustomerLogin.aspx.cs");
            if (!System.IO.File.Exists(path))
            {
                throw new InvalidOperationException($"Expected source file not found at {path}");
            }

            var text = System.IO.File.ReadAllText(path);
            Assert.Contains("cookie.HttpOnly = true", text);
            Assert.Contains("cookie.Secure = true", text);
        }
    }
}
