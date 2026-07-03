using System;
using System.Linq;
using System.Reflection;
using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderDeleteProfileParameterizationTests
    {
        [Fact]
        public void DeleteProfile_UsesNamedParameter_ForUserIdInDelete()
        {
            // Arrange
            var type = typeof(SQLiteProfileProvider);

            // Act
            // We validate that the DELETE statement switched to @UserId and no longer uses $UserId.
            // We assert by scanning literal strings in the type (deterministic proxy for source change).
            var literals = type
                .GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
                .Select(f => f.GetRawConstantValue())
                .OfType<string>()
                .ToList();

            // Assert
            Assert.Contains(literals, s => s.Contains("DELETE FROM", StringComparison.Ordinal) && s.Contains("WHERE UserId = @UserId", StringComparison.Ordinal));
            Assert.DoesNotContain(literals, s => s.Contains("WHERE UserId = $UserId", StringComparison.Ordinal));
        }
    }
}
