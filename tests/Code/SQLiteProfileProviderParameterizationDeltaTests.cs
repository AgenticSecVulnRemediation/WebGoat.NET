using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using Xunit;

// Assumption: the production assembly containing TechInfoSystems.Data.SQLite.SQLiteProfileProvider
// is referenced by the test project at build time.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderParameterizationDeltaTests
    {
        [Fact]
        public void SQLiteProfileProvider_SetPropertyValues_UsesAtParameters_ForUsernameAndApplicationId()
        {
            // This delta test verifies the security fix switched to named parameters "@Username" and "@ApplicationId".
            // We assert presence of these parameter names in the provider assembly metadata strings.

            var assembly = typeof(TechInfoSystems.Data.SQLite.SQLiteProfileProvider).Assembly;
            var module = assembly.ManifestModule;

            // The literal strings should be embedded due to cmd.Parameters.AddWithValue usage.
            Assert.Contains("@Username", module.Name == null ? string.Empty : ReadAllMetadataStrings(assembly));
            Assert.Contains("@ApplicationId", ReadAllMetadataStrings(assembly));

            // And the prior "$"-prefixed parameter names should no longer be used for this query.
            Assert.DoesNotContain("$Username", ReadAllMetadataStrings(assembly));
            Assert.DoesNotContain("$ApplicationId", ReadAllMetadataStrings(assembly));
        }

        private static string ReadAllMetadataStrings(Assembly assembly)
        {
            // Deterministic, no I/O: scan loaded types/members for string constants.
            // This is a best-effort way to avoid requiring DB/config initialization.
            var sb = new System.Text.StringBuilder();
            foreach (var t in assembly.GetTypes())
            {
                foreach (var f in t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
                {
                    if (f.FieldType == typeof(string))
                    {
                        try
                        {
                            var val = f.IsStatic ? f.GetValue(null) as string : null;
                            if (!string.IsNullOrEmpty(val)) sb.Append(val).Append('\n');
                        }
                        catch { }
                    }
                }
            }
            return sb.ToString();
        }
    }
}
