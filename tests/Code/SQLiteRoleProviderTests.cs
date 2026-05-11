using System;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderTests
    {
        [Fact]
        public void GetAllRoles_UsesAtApplicationIdParameter_InSqlText()
        {
            // Arrange
            const string expected = "SELECT RoleName FROM [aspnet_Roles] WHERE ApplicationId = @ApplicationId";

            // Assert
            Assert.Contains("@ApplicationId", expected);
            Assert.DoesNotContain("$ApplicationId", expected);
        }
    }
}
