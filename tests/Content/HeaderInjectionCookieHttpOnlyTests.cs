using System;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class HeaderInjectionCookieHttpOnlyTests
    {
        [Fact]
        public void HeaderInjection_PageLoad_MarksUserAddedCookie_HttpOnly()
        {
            // Delta regression: ensure HttpOnly is explicitly set when creating the cookie.
            var src = System.IO.File.ReadAllText("WebGoat/Content/HeaderInjection.aspx.cs");

            Assert.Contains("cookie.HttpOnly = true", src);
        }
    }
}
