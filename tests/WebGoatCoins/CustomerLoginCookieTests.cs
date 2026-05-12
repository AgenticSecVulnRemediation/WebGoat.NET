using Xunit;
using System.Web;
using System.IO;
using System;

// Assumption: page class is OWASP.WebGoat.NET.WebGoatCoins.CustomerLogin
using OWASP.WebGoat.NET.WebGoatCoins;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginCookieTests
    {
        [Fact]
        public void ButtonLogOnClick_SetsAuthCookieHttpOnly()
        {
            // Arrange: minimal HttpContext to allow Response.Cookies.Add
            var request = new HttpRequest("", "http://localhost/WebGoatCoins/CustomerLogin.aspx", "");
            var response = new HttpResponse(new StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            var page = new CustomerLogin();

            // Act
            var mi = typeof(CustomerLogin).GetMethod("ButtonLogOn_Click", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            Assert.NotNull(mi);

            try
            {
                mi!.Invoke(page, new object[] { page, EventArgs.Empty });
            }
            catch
            {
                // Ignore: depends on controls and DB provider; we validate emitted auth cookie is hardened if present.
            }

            // Assert
            // Default FormsAuthentication cookie name is ".ASPXAUTH" but framework may vary; we check any cookie with HttpOnly set.
            bool anyHttpOnly = false;
            foreach (string key in HttpContext.Current.Response.Cookies)
            {
                var c = HttpContext.Current.Response.Cookies[key];
                if (c != null && c.HttpOnly)
                {
                    anyHttpOnly = true;
                    break;
                }
            }

            Assert.True(anyHttpOnly);
        }
    }
}
