using System.Reflection;
using Xunit;

using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderGetAllRolesTests
    {
        [Fact]
        public void GetAllRoles_MethodExists_AfterParameterPrefixChange()
        {
            // Arrange
            // Delta: query parameter changed from $ApplicationId to @ApplicationId.
            // We validate the public method remains available.
            var method = typeof(SQLiteRoleProvider).GetMethod("GetAllRoles", BindingFlags.Public | BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }
    }
}
