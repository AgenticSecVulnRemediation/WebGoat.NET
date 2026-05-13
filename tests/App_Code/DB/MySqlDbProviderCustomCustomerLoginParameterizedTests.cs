using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderCustomCustomerLoginParameterizedTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesEmailParameter_InsteadOfConcatenation()
        {
            // Delta: query changed to parameterized @email.
            var source = SourceText.Read("WebGoat/App_Code/DB/MySqlDbProvider.cs");
            Assert.Contains("where email = @email", source);
            Assert.Contains("Parameters.AddWithValue(\"@email\"", source);
            Assert.DoesNotContain("where email = '\" + email", source);
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
