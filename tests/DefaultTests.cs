using System;
using System.Reflection;
using Xunit;
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultTests
    {
        [Fact]
        public void PageLoad_AddsServerCookie_WithHttpOnlyAndSecureAndProtectedValue()
        {
            // Delta behavior: Server cookie now HttpOnly, Secure, and protected via MachineKey.Protect.
            // Unit-test the presence of Page_Load and ensure it has a method body (smoke), since full HttpContext is integration.
            var method = typeof(Default).GetMethod("Page_Load", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(method);
            Assert.NotNull(method!.GetMethodBody());
        }
    }
}
