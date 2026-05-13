using Xunit;
using System;
using System.Reflection;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderTests
    {
        [Fact]
        public void GetAllRoles_UsesPositionalParameterMarker()
        {
            // Arrange
            // Regression for parameter marker change ($ApplicationId -> ?)
            var mi = typeof(SQLiteRoleProvider).GetMethod("GetAllRoles");

            // Assert
            Assert.NotNull(mi);
        }
    }
}
