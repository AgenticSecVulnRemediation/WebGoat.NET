using Xunit;
using System;
using System.Data;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderTests
    {
        // Delta test: GetAllRoles changed parameter placeholder from $ApplicationId to @ApplicationId.
        // This test ensures the provider still returns roles for the application without throwing due to
        // parameter name mismatch.
        [Fact]
        public void GetAllRoles_WithValidApplicationIdParameter_ReturnsRoles()
        {
            // Arrange: create a temp SQLite DB with the minimal schema needed.
            string dbPath = System.IO.Path.GetTempFileName();
            try
            {
                string connectionString = $"Data Source={dbPath};Version=3";

                using (var conn = new SqliteConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "CREATE TABLE aspnet_Roles (RoleId TEXT, RoleName TEXT, LoweredRoleName TEXT, ApplicationId TEXT);";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "INSERT INTO aspnet_Roles(RoleId, RoleName, LoweredRoleName, ApplicationId) VALUES ('r1','Admin','admin','app1');";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "INSERT INTO aspnet_Roles(RoleId, RoleName, LoweredRoleName, ApplicationId) VALUES ('r2','User','user','app1');";
                        cmd.ExecuteNonQuery();
                    }
                }

                // Build provider instance without calling Initialize (requires web.config); inject private static fields.
                var provider = new SQLiteRoleProvider();

                typeof(SQLiteRoleProvider).GetField("_connectionString", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
                    ?.SetValue(null, connectionString);
                typeof(SQLiteRoleProvider).GetField("_applicationId", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
                    ?.SetValue(null, "app1");

                // Act
                string[] roles = provider.GetAllRoles();

                // Assert
                Assert.NotNull(roles);
                Assert.Contains("Admin", roles);
                Assert.Contains("User", roles);
            }
            finally
            {
                try { System.IO.File.Delete(dbPath); } catch { }
            }
        }
    }
}
