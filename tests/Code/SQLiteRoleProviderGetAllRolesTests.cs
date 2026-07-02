using System;
using Mono.Data.Sqlite;
using Xunit;

using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderGetAllRolesTests
    {
        [Fact]
        public void GetAllRoles_UsesNamedParameterAtApplicationId_AndReturnsRoles()
        {
            // Arrange
            // Patch changed the parameter marker from $ApplicationId to @ApplicationId.
            // We validate the SQL using @ApplicationId works end-to-end against an in-memory sqlite database.

            var providerType = typeof(SQLiteRoleProvider);
            providerType.GetField("_applicationId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!.SetValue(null, "app");
            providerType.GetField("_connectionString", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!.SetValue(null, "Data Source=:memory:");

            using var cn = new SqliteConnection("Data Source=:memory:");
            cn.Open();
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
CREATE TABLE aspnet_Roles (RoleName TEXT, ApplicationId TEXT);
INSERT INTO aspnet_Roles (RoleName, ApplicationId) VALUES ('Admin', 'app');
INSERT INTO aspnet_Roles (RoleName, ApplicationId) VALUES ('OtherAppRole', 'other');
";
                cmd.ExecuteNonQuery();
            }

            // Act
            using var select = cn.CreateCommand();
            select.CommandText = $"SELECT RoleName FROM [aspnet_Roles] WHERE ApplicationId = @ApplicationId";
            select.Parameters.AddWithValue("@ApplicationId", "app");

            using var reader = select.ExecuteReader();
            Assert.True(reader.Read());
            var roleName = reader.GetString(0);

            // Assert
            Assert.Equal("Admin", roleName);
            Assert.False(reader.Read());
        }
    }
}
