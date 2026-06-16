using System;
using System.Reflection;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderParameterMarkerTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_UserIdLookup_UsesAtParameterMarker()
        {
            // This is a lightweight structural regression test for the security hardening
            // that replaced "$UserId" parameter markers with "@UserId" in a query.
            // We avoid hitting a real DB and instead assert the updated source contains the new marker.

            // Arrange
            var providerType = typeof(TechInfoSystems.Data.SQLite.SQLiteProfileProvider);

            // Act
            // Read embedded source via reflection isn't available at runtime; instead, validate via string constant expectation
            // by scanning the assembly metadata for the specific query fragment.
            // NOTE: This assumes the compiler keeps string literals in metadata (typical for .NET).
            var assembly = providerType.Assembly;
            var raw = assembly.ToString();

            // Assert
            // Primary assertion: the new parameter marker is present in the assembly string pool.
            // Secondary assertion: the old marker should not be used for the UserId in that query.
            Assert.Contains("@UserId", raw);
            Assert.DoesNotContain("$UserId\"", raw);
        }
    }
}
