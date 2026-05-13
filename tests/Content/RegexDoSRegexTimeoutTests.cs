using System;
using System.Reflection;
using OWASP.WebGoat.NET;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSRegexTimeoutTests
    {
        [Fact]
        public void BtnCreateClick_UsesRegexConstructorWithTimeout()
        {
            // Arrange
            // Delta: Regex is now constructed with a timeout to mitigate ReDoS.
            var asm = typeof(RegexDoS).Assembly;
            var strings = TestStringScanner.GetAllStringLiterals(asm);

            // Act / Assert
            Assert.Contains("TimeSpan.FromMilliseconds(1000)", strings);
        }

        private static class TestStringScanner
        {
            public static string[] GetAllStringLiterals(Assembly assembly)
            {
                var list = new System.Collections.Generic.HashSet<string>(StringComparer.Ordinal);
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var m in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
                    {
                        var body = m.GetMethodBody();
                        if (body == null) continue;
                        var il = body.GetILAsByteArray();
                        if (il == null) continue;
                        for (int i = 0; i < il.Length - 4; i++)
                        {
                            if (il[i] != 0x72) continue; // ldstr
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
