using System;
using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderGetAllRolesParameterStyleTests
    {
        [Fact]
        public void GetAllRoles_UsesAtApplicationIdParameterStyle_AndReturnsRoles()
        {
            // Arrange
            // Regression test for changing $ApplicationId to @ApplicationId.
            var provider = new SQLiteRoleProvider();

            var connStr = "Data Source=:memory:;Version=3;New=True;";
            SetStatic("_connectionString", connStr);

            var appId = Guid.NewGuid().ToString();
            SetStatic("_applicationId", appId);

            using (var cn = new Mono.Data.Sqlite.SqliteConnection(connStr))
            {
                cn.Open();
                using var cmd = cn.CreateCommand();
                cmd.CommandText = "CREATE TABLE [aspnet_Roles](RoleId TEXT, RoleName TEXT, ApplicationId TEXT);";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO [aspnet_Roles](RoleId, RoleName, ApplicationId) VALUES ($id,$name,$app);";
                cmd.Parameters.AddWithValue("$id", Guid.NewGuid().ToString());
                cmd.Parameters.AddWithValue("$name", "Admin");
                cmd.Parameters.AddWithValue("$app", appId);
                cmd.ExecuteNonQuery();
            }

            // Act
            var ex = Record.Exception(() => {
                var roles = provider.GetAllRoles();
                Assert.Contains("Admin", roles);
            });

            // Assert
            Assert.Null(ex);
        }

        private static void SetStatic(string fieldName, object value)
        {
            var f = typeof(SQLiteRoleProvider).GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(f);
            f!.SetValue(null, value);
        }
    }
}
