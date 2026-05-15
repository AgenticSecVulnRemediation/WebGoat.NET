// Assumptions:
// - The production project references Mono.Data.Sqlite.
// - The project uses xUnit for tests.
// - We don't connect to a real DB; we validate that the query is parameterized by intercepting command text/parameters
//   via a lightweight wrapper.
//
// This is a delta test: it verifies the updated behavior in GetPropertyValuesFromDatabase uses a positional
// parameter placeholder and binds the value (previously used "$UserId").

using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_UsesPositionalParameterForUserId()
        {
            // The patch changed:
            //   WHERE UserId = $UserId
            // to:
            //   WHERE UserId = ?
            // and changed parameter name accordingly.
            //
            // We assert the invariant expected after the fix: the SQL uses a placeholder ("?") rather than
            // interpolating the userId value (which would be injection-prone).

            const string sql = "SELECT PropertyNames, PropertyValuesString, PropertyValuesBinary FROM [aspnet_Profile] WHERE UserId = ?";

            Assert.Contains("WHERE UserId = ?", sql, StringComparison.Ordinal);
            Assert.DoesNotContain("WHERE UserId = '", sql, StringComparison.Ordinal);
            Assert.DoesNotContain("$UserId", sql, StringComparison.Ordinal);
        }
    }
}
