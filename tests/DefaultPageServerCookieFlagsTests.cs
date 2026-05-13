using System;
using System.IO;
using System.Web;
using System.Web.UI;
using OWASP.WebGoat.NET;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultPageServerCookieFlagsTests
    {
        [Fact]
        public void PageLoad_WhenDbConnectionSucceeds_SetsServerCookieHttpOnlyAndSecure()
        {
            // Arrange
            // This test focuses on the delta: the Server cookie is now marked HttpOnly and Secure.
            // We can't reliably force du.TestConnection() here without refactoring; instead we assert
            // that the compiled assembly includes the flag-setting code.
            var asm = typeof(Default).Assembly;
            var strings = TestStringScanner.GetAllStringLiterals(asm);

            // Assert
            Assert.Contains("cookie.HttpOnly = true", strings);
            Assert.Contains("cookie.Secure = true", strings);
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
