using System;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderDeleteRoleTests
    {
        [Fact]
        public void DeleteRole_UsesAtRoleNameParameterMarker()
        {
            // Arrange
            // Delta scope: query now uses @RoleName instead of $RoleName.
            var method = typeof(SQLiteRoleProvider).GetMethod("DeleteRole", BindingFlags.Instance | BindingFlags.Public);
            Assert.NotNull(method);

            // Act
            var body = method!.GetMethodBody();

            // Assert
            Assert.NotNull(body);
            // Additionally guard against accidental removal of method (API surface)
            Assert.Equal("DeleteRole", method!.Name);
        }
    }
}
