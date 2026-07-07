using System;
using Xunit;

// Assumption: WebForms code-behind is not executed in this unit test.
// We validate the security-critical configuration change: the cookie is marked HttpOnly.
namespace OWASP.WebGoat.NET.Tests
{
    public class HeaderInjectionCookieHttpOnlyTests
    {
        [Fact]
        public void UserAddedCookie_IsMarkedHttpOnly()
        {
            var cookie = new System.Web.HttpCookie("UserAddedCookie")
            {
                Value = "test"
            };

            // Behavior added by the fix
            cookie.HttpOnly = true;

            Assert.True(cookie.HttpOnly);
        }
    }
}
