using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailsParameterizedTests
    {
        [Fact]
        public void GetCustomerEmails_UsesParameterizedLikeAndAppendsWildcard()
        {
            // Delta: query uses parameterized LIKE with email + "%".
            var source = SourceText.Read("WebGoat/App_Code/DB/SqliteDbProvider.cs");
            Assert.Contains("where email like @email", source);
            Assert.Contains("AddWithValue(\"@email\", email + \"%\")", source);
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
