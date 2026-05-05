using System;
using System.Reflection;
using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderDeleteRoleTests
    {
        [Fact]
        public void DeleteRole_WhenDeletingUsersInRole_UsesAtRoleNameParameter_DoesNotThrow()
        {
            // Delta test: subquery parameter switched from $RoleName to @RoleName.

            // Arrange
            var provider = new SQLiteRoleProvider();

            var cs = "Data Source=file:roledb?mode=memory&cache=shared";
            SetStaticField(typeof(SQLiteRoleProvider), "_connectionString", cs);
            SetStaticField(typeof(SQLiteRoleProvider), "_applicationId", Guid.NewGuid().ToString());

            using (var cn = new Mono.Data.Sqlite.SqliteConnection(cs))
            {
                cn.Open();
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE aspnet_Roles (RoleId TEXT, LoweredRoleName TEXT, ApplicationId TEXT);";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "CREATE TABLE aspnet_UsersInRoles (UserId TEXT, RoleId TEXT);";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "INSERT INTO aspnet_Roles (RoleId, LoweredRoleName, ApplicationId) VALUES ($rid,$rn,$app);";
                    cmd.Parameters.AddWithValue("$rid", Guid.NewGuid().ToString());
                    cmd.Parameters.AddWithValue("$rn", "admin");
                    cmd.Parameters.AddWithValue("$app", GetStaticField<string>(typeof(SQLiteRoleProvider), "_applicationId"));
                    cmd.ExecuteNonQuery();
                }
            }

            // Force RoleExists to return true by creating required application/table state.
            // We'll call private DeleteRole method path directly; it will query RoleExists, which uses the same DB.

            // Act
            var ex = Record.Exception(() => provider.DeleteRole("Admin", throwOnPopulatedRole: false));

            // Assert
            Assert.Null(ex);
        }

        private static void SetStaticField(Type t, string fieldName, object value)
        {
            var f = t.GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic);
            Assert.NotNull(f);
            f!.SetValue(null, value);
        }

        private static T GetStaticField<T>(Type t, string fieldName)
        {
            var f = t.GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic);
            Assert.NotNull(f);
            return (T)f!.GetValue(null)!;
        }
    }
}
