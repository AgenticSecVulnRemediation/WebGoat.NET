using System;
using System.Reflection;
using Xunit;

// Assumption: the production assembly containing TechInfoSystems.Data.SQLite.SQLiteRoleProvider
// is referenced by the test project.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderDeleteRoleDeltaTests
    {
        [Fact]
        public void SQLiteRoleProvider_DeleteRole_UsesAtParameter_ForRoleName()
        {
            // Delta: parameter name changed from "$RoleName" to "@RoleName" in the delete statement.
            // Verify the new parameter name is present, and the old one is absent.

            var assembly = typeof(TechInfoSystems.Data.SQLite.SQLiteRoleProvider).Assembly;
            var allStrings = ReadAllMetadataStrings(assembly);

            Assert.Contains("@RoleName", allStrings);
            Assert.DoesNotContain("$RoleName", allStrings);
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
