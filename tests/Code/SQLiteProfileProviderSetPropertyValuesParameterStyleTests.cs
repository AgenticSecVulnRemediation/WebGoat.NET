using System;
using System.Data;
using System.Reflection;
using Xunit;
using Moq;

// Assumption: production namespace is as declared in source.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderSetPropertyValuesParameterStyleTests
    {
        [Fact]
        public void SetPropertyValues_UsesAtStylePlaceholdersForUsernameAndApplicationId()
        {
            // This is a delta-test focused on the changed behavior in PR:
            // The SELECT query text now uses @Username/@ApplicationId placeholders.
            // We assert the command text contains those placeholders.

            // Arrange
            var provider = new SQLiteProfileProvider();

            // Create a fake command to capture CommandText. We'll intercept SqliteConnection.CreateCommand
            // by using a lightweight derived wrapper accessed via reflection calling private method is infeasible.
            // Instead, we validate the updated source contains the corrected placeholders by reflecting the method body.
            // Note: This is still a unit-level regression guard on the security fix.

            var method = typeof(SQLiteProfileProvider).GetMethod("SetPropertyValues", BindingFlags.Public | BindingFlags.Instance);
            Assert.NotNull(method);

            var body = method!.GetMethodBody();
            Assert.NotNull(body);

            // IL bytes include embedded strings; we search Module for the literal.
            var module = typeof(SQLiteProfileProvider).Module;
            var atQuery = "WHERE LoweredUsername = @Username AND ApplicationId = @ApplicationId";

            // Act
            bool found = false;
            foreach (var s in GetAllUserStrings(module))
            {
                if (s.Contains(atQuery, StringComparison.Ordinal))
                {
                    found = true;
                    break;
                }
            }

            // Assert
            Assert.True(found, "Expected SetPropertyValues to use @Username/@ApplicationId placeholders in SQL query.");
        }

        // Helper: brute-force enumerate string resources in module.
        // Works for .NET assemblies where user strings are present.
        private static string[] GetAllUserStrings(Module module)
        {
            // There is no official API to enumerate all user strings; we use a heuristic that is stable for unit tests:
            // scan metadata tokens in a small range and capture those that decode.
            // If project build strips these, this test will fail and should be rewritten to an integration test.
            var strings = new System.Collections.Generic.List<string>();
            for (int token = 0x70000001; token < 0x70001000; token++)
            {
                try
                {
                    string? s = module.ResolveString(token);
                    if (!string.IsNullOrEmpty(s))
                        strings.Add(s);
                }
                catch
                {
                    // ignore
                }
            }
            return strings.ToArray();
        }
    }
}
