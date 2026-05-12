using System;
using System.Reflection;
using Xunit;

// Assumption: the production assembly containing TechInfoSystems.Data.SQLite.SQLiteProfileProvider
// is referenced by the test project.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderVerifyApplicationDeltaTests
    {
        [Fact]
        public void SQLiteProfileProvider_VerifyApplication_UsesPositionalParameters_InInsertStatement()
        {
            // Delta: INSERT statement changed from named parameters to positional placeholders.
            // Assert the placeholder pattern exists and the older named parameter tuple does not.

            var assembly = typeof(TechInfoSystems.Data.SQLite.SQLiteProfileProvider).Assembly;
            var allStrings = ReadAllMetadataStrings(assembly);

            Assert.Contains("VALUES (?, ?, ?)", allStrings);
            Assert.DoesNotContain("VALUES ($ApplicationId, $ApplicationName, $Description)", allStrings);
        }

        private static string ReadAllMetadataStrings(Assembly assembly)
        {
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
