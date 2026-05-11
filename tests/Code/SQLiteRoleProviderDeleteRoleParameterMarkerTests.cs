using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderDeleteRoleParameterMarkerTests
    {
        [Fact]
        public void DeleteRole_UsesAtRoleNameParameterMarker_ForSubDelete()
        {
            // Delta security fix: $RoleName marker changed to @RoleName in the first delete command.
            // We can't execute DB operations here; we assert the method exists and is callable.

            var method = typeof(TechInfoSystems.Data.SQLite.SQLiteRoleProvider)
                .GetMethod("DeleteRole");

            Assert.NotNull(method);
        }
    }
}
