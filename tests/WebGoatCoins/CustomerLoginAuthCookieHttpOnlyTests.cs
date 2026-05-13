using System;
using Xunit;

namespace OWASP.WebGoat.NET.Tests.WebGoatCoins
{
    public class CustomerLogin_SetsFormsAuthCookieHttpOnlyTests
    {
        [Fact]
        public void CustomerLogin_ButtonLogOn_Click_SetsAuthCookie_HttpOnlyTrue()
        {
            // Delta guard for PR #424: Forms auth cookie is now HttpOnly.
            var source = LoadSource();

            Assert.Contains("new HttpCookie(FormsAuthentication.FormsCookieName", source);
            Assert.Contains("cookie.HttpOnly = true", source);
        }

        private static string LoadSource()
        {
            var path = System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "WebGoat", "WebGoatCoins", "CustomerLogin.aspx.cs");
            path = System.IO.Path.GetFullPath(path);
            return System.IO.File.ReadAllText(path);
        }
    }
}
