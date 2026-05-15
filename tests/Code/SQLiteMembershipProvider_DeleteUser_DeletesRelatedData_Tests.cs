using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using System.Web.Security;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;
using Xunit;

// Delta-focused test: DeleteUser now uses interpolated constant table name; critical regression risk is SQL correctness.
// We validate it still deletes the user and related rows when deleteAllRelatedData=true.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProvider_DeleteUser_DeletesRelatedData_Tests
    {
        private static void SetStaticField(Type t, string name, object? value)
        {
            var f = t.GetField(name, BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(f);
            f!.SetValue(null, value);
        }

        private static SQLiteMembershipProvider CreateProviderWithSharedDb(string cs)
        {
            var p = new SQLiteMembershipProvider();
            // Avoid full Initialize; instead set required private static fields.
            SetStaticField(typeof(SQLiteMembershipProvider), "_connectionString", cs);
            SetStaticField(typeof(SQLiteMembershipProvider), "_applicationId", "app1");
            return p;
        }

        [Fact]
        public void DeleteUser_WithDeleteAllRelatedData_DeletesFromUsersRolesAndProfile()
        {
            var cs = "Data Source=file:memdb3?mode=memory&cache=shared";
            using var keeper = new SqliteConnection(cs);
            keeper.Open();

            using (var cmd = keeper.CreateCommand())
            {
                cmd.CommandText = @"
CREATE TABLE aspnet_Users (UserId TEXT, LoweredUsername TEXT, ApplicationId TEXT);
CREATE TABLE aspnet_UsersInRoles (UserId TEXT);
CREATE TABLE aspnet_Profile (UserId TEXT);
INSERT INTO aspnet_Users(UserId, LoweredUsername, ApplicationId) VALUES ('u1', 'alice', 'app1');
INSERT INTO aspnet_UsersInRoles(UserId) VALUES ('u1');
INSERT INTO aspnet_Profile(UserId) VALUES ('u1');
";
                cmd.ExecuteNonQuery();
            }

            var provider = CreateProviderWithSharedDb(cs);

            // Act
            var deleted = provider.DeleteUser("Alice", deleteAllRelatedData: true);

            // Assert
            Assert.True(deleted);
            using (var verify = keeper.CreateCommand())
            {
                verify.CommandText = "SELECT COUNT(*) FROM aspnet_Users";
                Assert.Equal(0L, (long)verify.ExecuteScalar()!);
                verify.CommandText = "SELECT COUNT(*) FROM aspnet_UsersInRoles";
                Assert.Equal(0L, (long)verify.ExecuteScalar()!);
                verify.CommandText = "SELECT COUNT(*) FROM aspnet_Profile";
                Assert.Equal(0L, (long)verify.ExecuteScalar()!);
            }
        }
    }
}
