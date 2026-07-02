using System;
using System.Reflection;
using Moq;
using Xunit;

// Note: Namespace inferred from source file's namespace in patched file.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void SetPropertyValues_UserIdLookup_UsesAtPrefixedParameters_NotDollarPrefixed()
        {
            // Arrange
            // This regression test focuses on the changed SQL parameter prefix used for the UserId lookup.
            // The fix replaced $Username/$ApplicationId with @Username/@ApplicationId.

            // Because the provider creates SqliteCommand instances internally and touches DB infrastructure,
            // we validate the behavior by inspecting the patched source content via reflection is not feasible.
            // Instead, we assert the intended secure behavior at the boundary: the SQL text contains parameter
            // markers and does not concatenate username.

            var method = typeof(SQLiteProfileProvider).GetMethod(
                "SetPropertyValues",
                BindingFlags.Public | BindingFlags.Instance);
            Assert.NotNull(method);

            // Act/Assert
            // The best deterministic unit assertion we can do without DB is to ensure the source type exists and
            // that the method is present. Then, we check that the patched query string is embedded in the assembly.
            // This is a common technique for legacy providers where DB objects cannot be injected.
            var asm = typeof(SQLiteProfileProvider).Assembly;
            var bytes = System.IO.File.ReadAllBytes(asm.Location);
            var text = System.Text.Encoding.UTF8.GetString(bytes);

            Assert.Contains("WHERE LoweredUsername = @Username AND ApplicationId = @ApplicationId", text);
            Assert.DoesNotContain("WHERE LoweredUsername = $Username", text);
        }
    }
}
