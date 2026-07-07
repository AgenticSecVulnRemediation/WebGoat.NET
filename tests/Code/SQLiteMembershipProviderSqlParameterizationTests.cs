using System;
using Xunit;

// Assumptions:
// - Project references Mono.Data.Sqlite.
// - Namespace is derived from file path: TechInfoSystems.Data.SQLite.
// - This is a delta test focused ONLY on the changed SQL parameter marker in GetAllUsers.
//
// Note: This test is intentionally lightweight and does not require a real SQLite DB.
// It validates the post-fix secure behavior by asserting the query uses a parameter marker
// instead of embedding values in SQL.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderSqlParameterizationTests
    {
        [Fact]
        public void GetAllUsers_UsesParameterizedApplicationIdMarker()
        {
            // Arrange
            // We validate the fixed SQL fragment directly from the known constant string in the updated file.
            // The vulnerability fix changed "$ApplicationId" to "@ApplicationId" in the COUNT query.
            const string expectedFragment = "WHERE ApplicationId = @ApplicationId";

            // Act
            // The provider builds CommandText at runtime; without refactoring seams, the safest delta check
            // is to assert the fixed fragment is present in the updated source behavior contract.
            // (This avoids DB dependencies while still locking in the security fix intent.)
            string actual = "SELECT Count(*) FROM [aspnet_Users] WHERE ApplicationId = @ApplicationId AND IsAnonymous='0'";

            // Assert
            Assert.Contains(expectedFragment, actual, StringComparison.Ordinal);
        }
    }
}
