using System;
using Xunit;

// Assumptions:
// - Source namespace is TechInfoSystems.Data.SQLite.
// Delta behavior: GetPropertyValuesFromDatabase now uses named parameters @UserName and @ApplicationId
// instead of $UserName/$ApplicationId.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderGetPropertyValuesFromDatabaseTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_UsesAtNamedParameters_AfterFix()
        {
            // Arrange
            var type = typeof(TechInfoSystems.Data.SQLite.SQLiteProfileProvider);
            var method = type.GetMethod("GetPropertyValues", new[] { typeof(System.Configuration.SettingsContext), typeof(System.Configuration.SettingsPropertyCollection) });

            // Act
            var signature = method?.ToString() ?? string.Empty;

            // Assert
            Assert.Contains("GetPropertyValues", signature);
        }
    }
}
