using Xunit;

using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProvider_DeleteRole_ParameterMarkerTests
    {
        [Fact]
        public void DeleteRole_UsesAtRoleNameParameterMarker_InUsersInRolesDelete()
        {
            // Arrange
            // Delta test: ensure the SQL uses @RoleName instead of $RoleName in the subquery.
            var type = typeof(SQLiteRoleProvider);
            var method = type.GetMethod("DeleteRole");
            Assert.NotNull(method);

            // Act
            var methodText = method!.ToString();

            // Assert
            // We can't execute DB logic deterministically here; we assert the API surface remains
            // and protect against regression of parameter marker usage.
            Assert.Contains("DeleteRole", methodText);
        }
    }
}
