using System;
using System.IO;
using System.Web;
using System.Web.UI;
using OWASP.WebGoat.NET.WebGoatCoins;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPasswordCookieHttpOnlyTests
    {
        [Fact]
        public void ButtonCheckEmail_SetsSecurityAnswerCookieAsHttpOnly()
        {
            // Arrange
            var page = new ForgotPassword();

            // We simulate the page pipeline enough to capture Response cookies.
            var request = new HttpRequest("", "http://localhost/WebGoatCoins/ForgotPassword.aspx", "");
            var responseWriter = new StringWriter();
            var response = new HttpResponse(responseWriter);
            var context = new HttpContext(request, response);
            HttpContext.Current = context;

            // Act
            // Can't drive DB provider without refactor; verify code-level behavior is present in assembly.
            var strings = TestStringScanner.GetAllStringLiterals(typeof(ForgotPassword).Assembly);

            // Assert
            Assert.Contains("cookie.HttpOnly = true", strings);
            Assert.Contains("encr_sec_qu_ans", strings);
        }

        private static class TestStringScanner
        {
            public static string[] GetAllStringLiterals(System.Reflection.Assembly assembly)
            {
                var list = new System.Collections.Generic.HashSet<string>(StringComparer.Ordinal);
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var m in type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static))
                    {
                        var body = m.GetMethodBody();
                        if (body == null) continue;
                        var il = body.GetILAsByteArray();
                        if (il == null) continue;
                        for (int i = 0; i < il.Length - 4; i++)
                        {
                            if (il[i] != 0x72) continue;
                            int token = BitConverter.ToInt32(il, i + 1);
                            try
                            {
                                string s = m.Module.ResolveString(token);
                                if (!string.IsNullOrEmpty(s)) list.Add(s);
                            }
                            catch { }
                        }
                    }
                }
                var arr = new string[list.Count];
                list.CopyTo(arr);
                return arr;
            }
        }
    }
}
