using Xunit;

using OWASP.WebGoat.NET.WebGoatCoins;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPasswordCookieHardeningStrictTests
    {
        [Fact]
        public void ButtonCheckEmail_SetsCookieHttpOnlySecureAndSameSiteStrict()
        {
            // Arrange/Assert
            Assert.Contains("cookie.HttpOnly = true", System.IO.File.ReadAllText(typeof(ForgotPassword).Module.FullyQualifiedName));
            Assert.Contains("cookie.Secure = true", System.IO.File.ReadAllText(typeof(ForgotPassword).Module.FullyQualifiedName));
            Assert.Contains("cookie.SameSite = System.Web.SameSiteMode.Strict", System.IO.File.ReadAllText(typeof(ForgotPassword).Module.FullyQualifiedName));
        }
    }
}
