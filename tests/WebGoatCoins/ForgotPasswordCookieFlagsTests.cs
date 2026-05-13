using System;
using System.Web;
using OWASP.WebGoat.NET.WebGoatCoins;
using Xunit;

namespace OWASP.WebGoat.NET.WebGoatCoins.Tests
{
    public class ForgotPasswordCookieFlagsTests
    {
        [Fact]
        public void Cookie_encr_sec_qu_ans_MustBeHttpOnlyAndSecure_WhenIssued()
        {
            // Delta: newly added cookie flags must be set.
            // We assert the exact fixed lines exist to prevent regression.

            var source = SourceText.Read("WebGoat/WebGoatCoins/ForgotPassword.aspx.cs");
            Assert.Contains("cookie.HttpOnly = true", source);
            Assert.Contains("cookie.Secure = true", source);
        }
    }

    internal static class SourceText
    {
        public static string Read(string resourcePath)
        {
            var asm = typeof(SourceText).Assembly;
            var normalized = resourcePath.Replace('/', '.').Replace('\\', '.');
            foreach (var name in asm.GetManifestResourceNames())
            {
                if (name.EndsWith(normalized, StringComparison.OrdinalIgnoreCase))
                {
                    using var s = asm.GetManifestResourceStream(name);
                    using var r = new System.IO.StreamReader(s!);
                    return r.ReadToEnd();
                }
            }
            throw new InvalidOperationException($"Embedded resource not found for '{resourcePath}'. Ensure the source file is embedded for this delta test.");
        }
    }
}
