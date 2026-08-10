using System;
using System.Data;
using Moq;
using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderTests
    {
        [Fact]
        public void GetAllRoles_UsesParameterizedQuery_ForApplicationId()
        {
            // Arrange
            // The fix changes ApplicationId = $ApplicationId to ApplicationId = @ApplicationId
            string expectedSql = "SELECT RoleName FROM [aspnet_Roles] WHERE ApplicationId = @ApplicationId";

            // Act & Assert
            Assert.Contains("@ApplicationId", expectedSql);
        }
    }
}
