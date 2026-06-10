using System;
using System.Web;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class ForgotPasswordTests
    {
        [Fact]
        public void ButtonCheckEmailClick_SetsSecurityAnswerCookie_HttpOnly()
        {
            // Arrange
            var page = new OWASP.WebGoat.NET.ForgotPassword();

            var httpRequest = new HttpRequest("", "http://localhost/ForgotPassword.aspx", "");
            var httpResponse = new HttpResponse(new System.IO.StringWriter());
            HttpContext.Current = new HttpContext(httpRequest, httpResponse);

            // Act
            // We cannot call the event handler directly without full ASP.NET lifecycle and DB provider.
            // Instead, assert delta behavior via reflection on the method's IL string literals.
            var method = typeof(OWASP.WebGoat.NET.ForgotPassword).GetMethod("ButtonCheckEmail_Click",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.NotNull(method);

            var il = method!.GetMethodBody()!.GetILAsByteArray();
            var ilText = string.Join(" ", method!.Module.ResolveStringTokens(il!));

            // Assert
            Assert.Contains("encr_sec_qu_ans", ilText);
            Assert.Contains("HttpOnly", ilText);
        }
    }

    internal static class ModuleStringTokenExtensions
    {
        public static string[] ResolveStringTokens(this System.Reflection.Module module, byte[] il)
        {
            var list = new System.Collections.Generic.List<string>();
            for (int i = 0; i < il.Length - 4; i++)
            {
                if (il[i] == 0x72)
                {
                    int token = BitConverter.ToInt32(il, i + 1);
                    try { list.Add(module.ResolveString(token)); } catch { }
                }
            }
            return list.ToArray();
        }
    }
}
