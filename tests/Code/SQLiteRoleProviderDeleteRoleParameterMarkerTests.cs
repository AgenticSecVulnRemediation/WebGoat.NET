using System;
using Xunit;

// Assumption: SQLiteRoleProvider is in namespace TechInfoSystems.Data.SQLite.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderDeleteRoleParameterMarkerTests
    {
        [Fact]
        public void DeleteRole_UsesAtParameters_DoesNotThrowForRoleNameWithQuote()
        {
            // Arrange
            var provider = new SQLiteRoleProvider();

            // Act
            var ex = Record.Exception(() => provider.DeleteRole("ro'le", throwOnPopulatedRole: false));

            // Assert
            // Previously, mismatched parameter markers could lead to runtime failures.
            Assert.Null(ex);
        }
    }
}
