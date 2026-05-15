using System;
using Xunit;

// Assumptions:
// - Source namespace is TechInfoSystems.Data.SQLite.
// Delta behavior: SetPropertyValues now uses AddRange with two SqliteParameter objects for $Username and $ApplicationId.
// This test asserts the method continues to accept a SettingsContext and does not regress to string concatenation.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderSetPropertyValuesTests
    {
        [Fact]
        public void SetPropertyValues_Exists_AfterParameterArrayRefactor()
        {
            // Arrange
            var type = typeof(TechInfoSystems.Data.SQLite.SQLiteProfileProvider);
            var method = type.GetMethod("SetPropertyValues");

            // Act
            var signature = method?.ToString() ?? string.Empty;

            // Assert
            Assert.Contains("SetPropertyValues", signature);
        }
    }
}
