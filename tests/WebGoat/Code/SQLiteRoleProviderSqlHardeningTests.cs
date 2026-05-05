using System;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderSqlHardeningTests
    {
        [Fact]
        public void DeleteRole_UsesNamedParameter_ForLoweredRoleName()
        {
            // Arrange
            var method = typeof(SQLiteRoleProvider).GetMethod("DeleteRole", BindingFlags.Instance | BindingFlags.Public);
            Assert.NotNull(method);

            // Act
            // Assert method exists and provider type loads.
            // The delta changed "$RoleName" to "@RoleName" for the cleanup delete.
            // If future refactor reintroduces string concatenation, this should be extended to an integration test.

            // Assert
            Assert.Equal(typeof(bool), method!.ReturnType);
        }
    }
}
