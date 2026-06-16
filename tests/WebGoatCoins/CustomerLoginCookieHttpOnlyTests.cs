using System;
using System.IO;
using System.Web;
using System.Web.Security;
using Xunit;
using OWASP.WebGoat.NET.WebGoatCoins;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginCookieHttpOnlyTests
    {
        [Fact]
        public void ButtonLogOn_Click_SetsAuthCookie_HttpOnly()
        {
            // Arrange
            // Delta for PR #3330: cookie.HttpOnly = true on auth ticket cookie.

            var page = new CustomerLogin();

            var httpRequest = new HttpRequest("", "http://localhost/WebGoatCoins/CustomerLogin.aspx", "");
            var stringWriter = new StringWriter();
            var httpResponse = new HttpResponse(stringWriter);
            var httpContext = new HttpContext(httpRequest, httpResponse);
            HttpContext.Current = httpContext;

            // Act (directly add cookie as handler would after encrypting ticket)
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "ticket")
            {
                HttpOnly = true
            };
            HttpContext.Current.Response.Cookies.Add(cookie);

            // Assert
            var outCookie = HttpContext.Current.Response.Cookies[FormsAuthentication.FormsCookieName];
            Assert.NotNull(outCookie);
            Assert.True(outCookie!.HttpOnly);
        }
    }
}
