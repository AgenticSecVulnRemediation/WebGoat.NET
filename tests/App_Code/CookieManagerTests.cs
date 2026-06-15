using Xunit;
using OWASP.WebGoat.NET.App_Code;
using System.Web;
using System.Web.Security;

// Assumption: CookieManager is in namespace OWASP.WebGoat.NET.App_Code (from source file).
namespace OWASP.WebGoat.NET.App_Code.Tests
{
    public class CookieManagerTests
    {
        [Fact]
        public void SetCookie_WhenCalled_SetsHttpOnlyTrue()
        {
            // Arrange
            var ticket = new FormsAuthenticationTicket(
                1,
                "user@example.com",
                System.DateTime.UtcNow,
                System.DateTime.UtcNow.AddMinutes(30),
                true,
                "customer");

            // Act
            HttpCookie cookie = CookieManager.SetCookie(ticket, "ignored", "ignored");

            // Assert
            Assert.NotNull(cookie);
            Assert.True(cookie.HttpOnly);
        }
    }
}
