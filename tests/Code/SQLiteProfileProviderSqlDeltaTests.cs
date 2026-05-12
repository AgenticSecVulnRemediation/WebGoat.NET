// Assumptions:
// - The production project is under a top-level folder named "WebGoat".
// - The test project follows the convention "tests/..." as used in this repository.
// - This delta test validates the specific change in SQLiteProfileProvider where string interpolation is used
//   to build the SQL with the USER_TB_NAME constant while still using parameters ($Username / $UserName).

using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderSqlDeltaTests
    {
        [Fact]
        public void SQLiteProfileProvider_SetPropertyValues_UsesParameterizedUserLookupQueryText()
        {
            // Arrange
            var asm = typeof(TechInfoSystems.Data.SQLite.SQLiteProfileProvider).Assembly;

            // Act
            var allStrings = GetAllUserStrings(asm);

            // Assert (delta): ensure the parameterized query text is present after the fix.
            Assert.Contains("SELECT UserId FROM [aspnet_Users] WHERE LoweredUsername = $Username AND ApplicationId = $ApplicationId", allStrings);
        }

        [Fact]
        public void SQLiteProfileProvider_GetPropertyValuesFromDatabase_UsesParameterizedUserLookupQueryText()
        {
            // Arrange
            var asm = typeof(TechInfoSystems.Data.SQLite.SQLiteProfileProvider).Assembly;

            // Act
            var allStrings = GetAllUserStrings(asm);

            // Assert (delta): ensure the parameterized query text is present after the fix.
            Assert.Contains("SELECT UserId FROM [aspnet_Users] WHERE LoweredUsername = $UserName AND ApplicationId = $ApplicationId", allStrings);
        }

        private static string GetAllUserStrings(Assembly asm)
        {
            // Deterministic, no DB/network: scan metadata user strings.
            // If the runtime disallows reading the assembly bytes (e.g., single-file publish), we skip to avoid false negatives.
            var location = asm.Location;
            if (string.IsNullOrWhiteSpace(location) || !System.IO.File.Exists(location))
            {
                return string.Empty;
            }

            var bytes = System.IO.File.ReadAllBytes(location);
            // Interpret as UTF-8 for a best-effort search of embedded strings.
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
    }
}
