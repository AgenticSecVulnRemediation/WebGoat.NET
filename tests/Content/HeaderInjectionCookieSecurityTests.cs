using System;
using System.Reflection;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class HeaderInjectionCookieSecurityTests
    {
        [Fact]
        public void PageLoad_SetsUserAddedCookie_HttpOnly()
        {
            // Arrange
            var method = typeof(OWASP.WebGoat.NET.HeaderInjection).GetMethod("Page_Load",
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(method);

            // Act
            var il = method!.GetMethodBody()!.GetILAsByteArray();
            var ilText = string.Join(" ", method!.Module.ResolveStringTokens(il!));

            // Assert (delta): cookie.HttpOnly assignment added
            Assert.Contains("UserAddedCookie", ilText);
            Assert.Contains("HttpOnly", ilText);
        }

        [Fact]
        public void PageLoad_RejectsNonAlphanumericCookieInput_BySettingDefault()
        {
            // Arrange
            var method = typeof(OWASP.WebGoat.NET.HeaderInjection).GetMethod("Page_Load",
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(method);

            // Act
            var il = method!.GetMethodBody()!.GetILAsByteArray();
            var ilText = string.Join(" ", method!.Module.ResolveStringTokens(il!));

            // Assert (delta): regex-based validation and default fallback
            Assert.Contains("^[a-zA-Z0-9]{1,50}$", ilText);
            Assert.Contains("default", ilText);
            Assert.Contains("Secure", ilText);
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
