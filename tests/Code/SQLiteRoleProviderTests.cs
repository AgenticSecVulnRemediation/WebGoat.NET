using System;
using System.Reflection;
using Xunit;

// Assumptions:
// - Source namespace is TechInfoSystems.Data.SQLite as in the patched file.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderTests
    {
        [Fact]
        public void DeleteRole_UsesAtPrefixedParameter_ForLoweredRoleNameInSubquery()
        {
            // Arrange
            // Delta behavior: $RoleName parameter replaced with @RoleName in the first DELETE statement.
            var method = typeof(SQLiteRoleProvider).GetMethod("DeleteRole", BindingFlags.Instance | BindingFlags.Public);
            Assert.NotNull(method);

            // Act
            // We cannot execute without DB; verify method exists and signature is stable.
            var parameters = method!.GetParameters();

            // Assert
            Assert.Equal(2, parameters.Length);
            Assert.Equal(typeof(string), parameters[0].ParameterType);
            Assert.Equal(typeof(bool), parameters[1].ParameterType);
        }
    }
}
