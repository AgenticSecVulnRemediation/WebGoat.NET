using System;
using System.Reflection;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class CustomerLoginCookieSecurityTests
    {
        [Fact]
        public void ButtonLogOnClick_SetsFormsAuthCookie_HttpOnly()
        {
            // Arrange
            var method = typeof(OWASP.WebGoat.NET.WebGoatCoins.CustomerLogin).GetMethod("ButtonLogOn_Click",
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(method);

            // Act
            var il = method!.GetMethodBody()!.GetILAsByteArray();
            var ilText = string.Join(" ", method!.Module.ResolveStringTokens(il!));

            // Assert (delta)
            Assert.Contains("HttpOnly", ilText);
            Assert.Contains("FormsCookieName", ilText);
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
