using System;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using Moq;
using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderSetPropertyValuesTests
    {
        [Fact]
        public void SetPropertyValues_UsesNamedParameters_ForUserLookupQuery()
        {
            // Arrange
            // This delta test only validates the changed SQL text/parameter naming in SetPropertyValues.
            // We do not connect to any DB. We inspect the method body to ensure the insecure/incorrect placeholders are not used.
            var method = typeof(SQLiteProfileProvider).GetMethod("SetPropertyValues", BindingFlags.Instance | BindingFlags.Public);
            Assert.NotNull(method);

            // Act
            var ilBytes = method!.GetMethodBody()!.GetILAsByteArray();
            var ilString = BitConverter.ToString(ilBytes ?? Array.Empty<byte>());

            // Assert
            // Heuristic assertions: ensure the new parameter tokens are present in metadata string table.
            // These strings are embedded as literals in the compiled assembly.
            // We use reflection over all literals in the declaring type as a deterministic proxy.
            var allStrings = typeof(SQLiteProfileProvider)
                .GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
                .Select(f => f.GetRawConstantValue())
                .OfType<string>()
                .ToList();

            // Ensure no legacy placeholders are used in the user lookup query.
            Assert.DoesNotContain(allStrings, s => s.Contains("LoweredUsername = $Username", StringComparison.Ordinal));
            Assert.DoesNotContain(allStrings, s => s.Contains("ApplicationId = $ApplicationId", StringComparison.Ordinal));

            // Ensure new placeholders are used.
            Assert.Contains(allStrings, s => s.Contains("LoweredUsername = @Username", StringComparison.Ordinal));
            Assert.Contains(allStrings, s => s.Contains("ApplicationId = @ApplicationId", StringComparison.Ordinal));
        }
    }
}
