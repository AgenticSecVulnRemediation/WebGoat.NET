using System;
using System.Reflection;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderGetAllRolesTests
    {
        [Fact]
        public void GetAllRoles_UsesApplicationIdParameter_ReturnsRolesForApplication()
        {
            // Arrange
            // Delta test for GetAllRoles(): ApplicationId parameter name changed from $ApplicationId to @ApplicationId.
            // Validate behavior still returns roles for current application id.

            var provider = new SQLiteRoleProvider();

            var dbPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "wg_roles_" + Guid.NewGuid() + ".db");
            var cs = $"Data Source={dbPath};Version=3";

            using (var cn = new SqliteConnection(cs))
            {
                cn.Open();
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = @"
CREATE TABLE aspnet_Roles (
  RoleId TEXT PRIMARY KEY,
  RoleName TEXT,
  LoweredRoleName TEXT,
  ApplicationId TEXT
);
";
                    cmd.ExecuteNonQuery();
                }

                var appId = "app';--"; // ensure special chars don't break anything
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO aspnet_Roles(RoleId, RoleName, LoweredRoleName, ApplicationId) VALUES (@id,@n,@ln,@app)";
                    cmd.Parameters.AddWithValue("@id", "r1");
                    cmd.Parameters.AddWithValue("@n", "Admin");
                    cmd.Parameters.AddWithValue("@ln", "admin");
                    cmd.Parameters.AddWithValue("@app", appId);
                    cmd.ExecuteNonQuery();
                }

                typeof(SQLiteRoleProvider).GetField("_connectionString", BindingFlags.NonPublic | BindingFlags.Static)!.SetValue(null, cs);
                typeof(SQLiteRoleProvider).GetField("_applicationId", BindingFlags.NonPublic | BindingFlags.Static)!.SetValue(null, appId);

                // Act
                var roles = provider.GetAllRoles();

                // Assert
                Assert.Single(roles);
                Assert.Equal("Admin", roles[0]);
            }

            try { System.IO.File.Delete(dbPath); } catch { /* ignore */ }
        }
    }
}
