using System;
using System.Reflection;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultCookieSigningTests
    {
        [Fact]
        public void VerifyServerCookie_PrivateHelperExists_AfterSigningChange()
        {
            var method = typeof(OWASP.WebGoat.NET.Default)
                .GetMethod("VerifyServerCookie", BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.NotNull(method);
        }
    }
}
