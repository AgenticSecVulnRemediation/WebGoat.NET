using System;
using System.Reflection;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DefaultCookieSecurityTests
    {
        [Fact]
        public void PageLoad_SetsServerCookie_HttpOnly()
        {
            // Arrange
            var method = typeof(OWASP.WebGoat.NET.Default).GetMethod("Page_Load", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(method);

            // Act
            var il = method!.GetMethodBody()!.GetILAsByteArray();
            var ilText = string.Join(" ", method!.Module.ResolveStringTokens(il!));

            // Assert
            Assert.Contains("Server", ilText);
            Assert.Contains("HttpOnly", ilText);
        }
    }

    internal static class ModuleStringTokenExtensions
    {
        public static string[] ResolveStringTokens(this Module module, byte[] il)
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
