using System;
using System.Collections.Specialized;
using System.Web.Security;
using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderDeleteRoleParameterBindingTests
    {
        [Fact]
        public void DeleteRole_UsesNamedParameters_RoleNameAndApplicationId_AtRuntime()
        {
            // Arrange
            // Delta for PR #3331: changed DeleteRole delete query to use @RoleName and @ApplicationId.
            // Validate that query executes successfully against a minimal schema when parameters are used.

            var provider = new SQLiteRoleProvider();

            var connectionString = "Data Source=:memory:;Version=3;New=True;";
            SetStaticField(typeof(SQLiteRoleProvider), "_connectionString", connectionString);
            SetStaticField(typeof(SQLiteRoleProvider), "_applicationId", "app");
            SetStaticField(typeof(SQLiteRoleProvider), "_membershipApplicationId", "app");
            SetStaticField(typeof(SQLiteRoleProvider), "_applicationName", "/");
            SetStaticField(typeof(SQLiteRoleProvider), "_membershipApplicationName", "/");

            using var cn = new Mono.Data.Sqlite.SqliteConnection(connectionString);
            cn.Open();

            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
CREATE TABLE aspnet_Roles (RoleId TEXT, RoleName TEXT, LoweredRoleName TEXT, ApplicationId TEXT);
CREATE TABLE aspnet_UsersInRoles (UserId TEXT, RoleId TEXT);
INSERT INTO aspnet_Roles(RoleId, RoleName, LoweredRoleName, ApplicationId) VALUES ('r1', 'Admins', 'admins', 'app');
";
                cmd.ExecuteNonQuery();
            }

            var httpContext = new System.Web.HttpContext(
                new System.Web.HttpRequest("", "http://localhost", ""),
                new System.Web.HttpResponse(new System.IO.StringWriter()));
            System.Web.HttpContext.Current = httpContext;

            using var tran = cn.BeginTransaction();
            System.Web.HttpContext.Current.Items[GetConstString(typeof(SQLiteRoleProvider), "HTTP_TRANSACTION_ID")] = tran;

            // Act
            var result = provider.DeleteRole("Admins", throwOnPopulatedRole: false);

            // Assert
            Assert.True(result);

            using (var verifyCmd = cn.CreateCommand())
            {
                verifyCmd.CommandText = "SELECT COUNT(*) FROM aspnet_Roles WHERE ApplicationId='app'";
                var count = Convert.ToInt64(verifyCmd.ExecuteScalar());
                Assert.Equal(0, count);
            }
        }

        private static void SetStaticField(Type t, string fieldName, object? value)
        {
            var f = t.GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(f);
            f!.SetValue(null, value);
        }

        private static string GetConstString(Type t, string fieldName)
        {
            var f = t.GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(f);
            return (string)f!.GetValue(null)!;
        }
    }
}
