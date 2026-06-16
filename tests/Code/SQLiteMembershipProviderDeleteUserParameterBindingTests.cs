using System;
using System.Collections.Specialized;
using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserParameterBindingTests
    {
        [Fact]
        public void DeleteUser_DeleteAllRelatedData_UsesNamedUserIdParameterAtRuntime()
        {
            // Arrange
            // This is a delta test for PR #3326: changed DeleteUser related deletes from $UserId to @UserId.
            // We validate runtime behavior by ensuring ExecuteNonQuery succeeds with @UserId.
            // Provider is hard to fully initialize without config; this test focuses on SQL parameter style by
            // exercising the command text via reflection on a minimal in-memory SQLite DB.

            var provider = new SQLiteMembershipProvider();

            // Set the private static fields required by GetDBConnectionForMembership and DeleteUser.
            var connectionString = "Data Source=:memory:;Version=3;New=True;";
            SetStaticField(typeof(SQLiteMembershipProvider), "_connectionString", connectionString);
            SetStaticField(typeof(SQLiteMembershipProvider), "_applicationId", "app");

            using var cn = new Mono.Data.Sqlite.SqliteConnection(connectionString);
            cn.Open();

            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
CREATE TABLE aspnet_Users (UserId TEXT, LoweredUsername TEXT, ApplicationId TEXT);
CREATE TABLE aspnet_UsersInRoles (UserId TEXT, RoleId TEXT);
CREATE TABLE aspnet_Profile (UserId TEXT, PropertyNames TEXT);
INSERT INTO aspnet_Users(UserId, LoweredUsername, ApplicationId) VALUES ('u1', 'alice', 'app');
INSERT INTO aspnet_UsersInRoles(UserId, RoleId) VALUES ('u1', 'r1');
INSERT INTO aspnet_Profile(UserId, PropertyNames) VALUES ('u1', 'p');
";
                cmd.ExecuteNonQuery();
            }

            // Put the connection in HttpContext transaction slot so provider uses our open connection.
            // (This avoids provider creating a separate connection to a different in-memory database.)
            var httpContext = new System.Web.HttpContext(
                new System.Web.HttpRequest("", "http://localhost", ""),
                new System.Web.HttpResponse(new System.IO.StringWriter()));
            System.Web.HttpContext.Current = httpContext;

            using var tran = cn.BeginTransaction();
            System.Web.HttpContext.Current.Items[GetStaticField<string>(typeof(SQLiteMembershipProvider), "_httpTransactionId")] = tran;

            // Act
            var deleted = provider.DeleteUser("Alice", deleteAllRelatedData: true);

            // Assert
            Assert.True(deleted);

            using (var verifyCmd = cn.CreateCommand())
            {
                verifyCmd.CommandText = "SELECT COUNT(*) FROM aspnet_UsersInRoles WHERE UserId = 'u1'";
                var remainingUir = Convert.ToInt64(verifyCmd.ExecuteScalar());
                Assert.Equal(0, remainingUir);

                verifyCmd.CommandText = "SELECT COUNT(*) FROM aspnet_Profile WHERE UserId = 'u1'";
                var remainingProfile = Convert.ToInt64(verifyCmd.ExecuteScalar());
                Assert.Equal(0, remainingProfile);
            }
        }

        private static void SetStaticField(Type t, string fieldName, object? value)
        {
            var f = t.GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(f);
            f!.SetValue(null, value);
        }

        private static T GetStaticField<T>(Type t, string fieldName)
        {
            var f = t.GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(f);
            return (T)f!.GetValue(null)!;
        }
    }
}
