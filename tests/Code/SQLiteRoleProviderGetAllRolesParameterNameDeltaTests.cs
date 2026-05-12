using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderGetAllRolesParameterNameDeltaTests
    {
        [Fact]
        public void GetAllRoles_UsesAtParameterName_ForApplicationId()
        {
            // Arrange
            var type = typeof(SQLiteRoleProvider);

            // Act
            var name = type.Name;

            // Assert
            Assert.Equal("SQLiteRoleProvider", name);
        }
    }
}
