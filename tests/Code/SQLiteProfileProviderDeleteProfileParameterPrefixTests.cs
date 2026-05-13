using System;
using System.Data;
using Mono.Data.Sqlite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderDeleteProfileParameterPrefixTests
    {
        [Fact]
        public void DeleteProfile_CommandText_UsesAtParameters_NotDollarParameters()
        {
            // This is a strict delta regression test: the patch changed parameter placeholders
            // in DeleteProfile from $Username/$ApplicationId/$UserId to @Username/@ApplicationId/@UserId.
            // We validate by scanning the updated source text embedded as a resource.
            // Assumption: test project includes the production source file as a linked file resource named exactly as file path.

            var source = SourceText.Read("WebGoat/Code/SQLiteProfileProvider.cs");

            Assert.Contains("LoweredUsername = @Username", source);
            Assert.Contains("ApplicationId = @ApplicationId", source);
            Assert.Contains("WHERE UserId = @UserId", source);

            Assert.DoesNotContain("LoweredUsername = $Username", source);
            Assert.DoesNotContain("ApplicationId = $ApplicationId", source);
            Assert.DoesNotContain("WHERE UserId = $UserId", source);
        }
    }

    internal static class SourceText
    {
        public static string Read(string resourcePath)
        {
            // Minimal, deterministic helper: if the build doesn't embed sources,
            // this fails clearly and forces alignment rather than silently passing.
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
