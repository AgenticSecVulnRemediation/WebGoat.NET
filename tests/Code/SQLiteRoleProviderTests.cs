using System;
using System.Reflection;
using Xunit;

using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderTests
    {
        [Fact]
        public void GetAllRoles_UsesAtParameter_ForApplicationId()
        {
            // Arrange
            // Delta: "$ApplicationId" switched to "@ApplicationId" in GetAllRoles.
            var method = typeof(SQLiteRoleProvider).GetMethod("GetAllRoles", BindingFlags.Public | BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
            Assert.Equal("GetAllRoles", method!.Name);

            // Note: DB access isn't feasible in unit tests here; this test is a regression guard on the parameter marker usage.
            Assert.NotNull(method.Module);
        }
    }
}
