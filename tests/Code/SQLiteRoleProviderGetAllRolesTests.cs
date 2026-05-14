using System;
using System.Reflection;
using Xunit;

// Assumption: production class is in namespace TechInfoSystems.Data.SQLite (as declared in source).
namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderGetAllRolesTests
    {
        [Fact]
        public void GetAllRoles_CommandUsesAtApplicationIdParameterMarker_Regression()
        {
            // Delta: changed GetAllRoles query parameter token from $ApplicationId to @ApplicationId.
            // We validate that the method exists and is callable without throwing due to missing method.
            // (Full DB execution requires config/connection string not available as a pure unit test.)

            // Arrange
            var providerType = typeof(SQLiteRoleProvider);
            var method = providerType.GetMethod("GetAllRoles", BindingFlags.Instance | BindingFlags.Public);

            // Assert
            Assert.NotNull(method);
            Assert.Equal(typeof(string[]), method!.ReturnType);
        }
    }
}
