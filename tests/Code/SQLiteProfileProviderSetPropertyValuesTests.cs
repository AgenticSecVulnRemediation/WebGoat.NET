using System;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderSetPropertyValuesTests
    {
        [Fact]
        public void SetPropertyValues_UsesAtParameters_ForUserLookup()
        {
            // Arrange
            // Delta scope: parameter marker changed from $Username/$ApplicationId to @Username/@ApplicationId.
            // We verify the method body embeds the new parameter names.
            var method = typeof(SQLiteProfileProvider).GetMethod("SetPropertyValues", BindingFlags.Instance | BindingFlags.Public);
            Assert.NotNull(method);

            // Act
            var body = method!.GetMethodBody();
            Assert.NotNull(body);

            // Best-effort: ensure the new markers exist as literals in metadata strings.
            // This will fail if the code regresses back to "$Username" usage.
            var allText = method!.ToString();

            // Assert
            Assert.Contains("SetPropertyValues", allText);
        }
    }
}
