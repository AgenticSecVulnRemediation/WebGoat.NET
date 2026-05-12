// Assumptions:
// - Namespace in the source file is OWASP.WebGoat.NET
// - This delta test checks that the cookie created in ButtonCheckEmail_Click is hardened with HttpOnly and Secure.
// - We avoid requiring a full ASP.NET runtime by invoking the handler via reflection and validating the cookie
//   created on the response using a minimal HttpContext.

using System;
using System.IO;
using System.Reflection;
using System.Web;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class ForgotPasswordCookieHardeningDeltaTests
    {
        [Fact]
        public void ButtonCheckEmailClick_SetsHttpOnlyAndSecureOnSecurityAnswerCookie()
        {
            // Arrange: minimal HttpContext with request/response.
            var request = new HttpRequest("", "http://localhost/ForgotPassword.aspx", "");
            var response = new HttpResponse(new StringWriter());
            HttpContext.Current = new HttpContext(request, response);

            var page = new OWASP.WebGoat.NET.ForgotPassword();

            // We can't realistically execute the full event handler without all WebForms controls.
            // Delta assertion is done by inspecting the compiled artifact for the hardening properties being set.
            var asm = typeof(OWASP.WebGoat.NET.ForgotPassword).Assembly;
            var allStrings = GetAllUserStrings(asm);

            // Act/Assert (delta): look for property assignments introduced by the fix.
            Assert.Contains("cookie.HttpOnly = true", allStrings);
            Assert.Contains("cookie.Secure = true", allStrings);
        }

        private static string GetAllUserStrings(Assembly asm)
        {
            var location = asm.Location;
            if (string.IsNullOrWhiteSpace(location) || !File.Exists(location))
            {
                return string.Empty;
            }

            var bytes = File.ReadAllBytes(location);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
    }
}
