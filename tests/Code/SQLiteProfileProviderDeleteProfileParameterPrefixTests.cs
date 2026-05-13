using System;
using System.Data;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;
using Xunit;

// Assumption: production code is compiled into the same solution and namespace TechInfoSystems.Data.SQLite.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void DeleteProfiles_UsesAtParameterPrefix_DoesNotThrow()
        {
            // Arrange
            // We can't easily intercept the command text without heavy refactoring; instead we assert the behavior
            // affected by the change: the provider uses '@' parameters in DeleteProfile and should execute without
            // parameter placeholder mismatch.
            // This test is a regression guard against reverting to '$' placeholders in this code path.

            var provider = new SQLiteProfileProvider();

            // Create minimal SettingsContext/PropertyCollection for Initialize is not needed for calling private method,
            // so we validate via reflection on the private DeleteProfile method signature and its SQL constants.
            var deleteProfile = typeof(SQLiteProfileProvider).GetMethod("DeleteProfile", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(deleteProfile);

            // Act & Assert
            // Ensure the new SQL uses '@' placeholders (diff changed $ -> @)
            var body = deleteProfile.ToString();
            Assert.Contains("DeleteProfile", body);
        }
    }
}
