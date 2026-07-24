using System;
using System.Data;
using Xunit;
using Moq;

// Assumption: production namespace is as declared in source.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderDeleteProfileParameterStyleTests
    {
        [Fact]
        public void DeleteProfile_UsesAtStyleUserIdParameterInDeleteQuery()
        {
            // Arrange
            // Delta-test focused on the changed DELETE statement in DeleteProfile:
            // now uses "WHERE UserId = @UserId" (was $UserId).

            var method = typeof(SQLiteProfileProvider).GetMethod("DeleteProfiles", new[] { typeof(string[]) });
            Assert.NotNull(method);

            // Act
            var module = typeof(SQLiteProfileProvider).Module;
            var deleteQuery = "DELETE FROM [aspnet_Profile] WHERE UserId = @UserId";

            bool found = false;
            foreach (var s in GetAllUserStrings(module))
            {
                if (s.Contains(deleteQuery, StringComparison.Ordinal))
                {
                    found = true;
                    break;
                }
            }

            // Assert
            Assert.True(found, "Expected DeleteProfile to use @UserId parameter placeholder in DELETE query.");
        }

        private static string[] GetAllUserStrings(System.Reflection.Module module)
        {
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
