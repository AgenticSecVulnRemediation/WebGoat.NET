using System;
using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserParameterStyleTests
    {
        [Fact]
        public void DeleteUser_WhenDeletingRelatedData_UsesAtUserIdParameterStyle()
        {
            // Arrange
            // This is a regression test for the delta change from "$UserId" to "@UserId".
            // We can't directly intercept SqliteCommand, so we validate that the fixed source uses @UserId
            // by executing the DeleteUser path against a minimal in-memory SQLite schema.
            // If the provider still used $UserId but added @UserId, ExecuteNonQuery would throw.

            var provider = new SQLiteMembershipProvider();

            // The provider requires initialization to set connection string and application id.
            // We set private static fields via reflection and create required tables.
            var connStr = "Data Source=:memory:;Version=3;New=True;";
            SetStatic("_connectionString", connStr);
            SetStatic("_applicationId", Guid.NewGuid().ToString());

            // Create DB schema
            using (var cn = new Mono.Data.Sqlite.SqliteConnection(connStr))
            {
                cn.Open();
                using var cmd = cn.CreateCommand();
                cmd.CommandText = "CREATE TABLE [aspnet_Users](UserId TEXT, LoweredUsername TEXT, ApplicationId TEXT);";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CREATE TABLE [aspnet_UsersInRoles](UserId TEXT, RoleId TEXT);";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CREATE TABLE [aspnet_Profile](UserId TEXT);";
                cmd.ExecuteNonQuery();

                var userId = Guid.NewGuid().ToString();
                cmd.CommandText = "INSERT INTO [aspnet_Users](UserId, LoweredUsername, ApplicationId) VALUES ($uid,$un,$app);";
                cmd.Parameters.AddWithValue("$uid", userId);
                cmd.Parameters.AddWithValue("$un", "bob");
                cmd.Parameters.AddWithValue("$app", GetStatic("_applicationId"));
                cmd.ExecuteNonQuery();

                cmd.Parameters.Clear();
                cmd.CommandText = "INSERT INTO [aspnet_Profile](UserId) VALUES ($uid);";
                cmd.Parameters.AddWithValue("$uid", userId);
                cmd.ExecuteNonQuery();

                cmd.Parameters.Clear();
                cmd.CommandText = "INSERT INTO [aspnet_UsersInRoles](UserId, RoleId) VALUES ($uid,$rid);";
                cmd.Parameters.AddWithValue("$uid", userId);
                cmd.Parameters.AddWithValue("$rid", Guid.NewGuid().ToString());
                cmd.ExecuteNonQuery();
            }

            // Act + Assert
            // Should not throw due to parameter placeholder mismatch.
            var ex = Record.Exception(() => provider.DeleteUser("Bob", deleteAllRelatedData: true));
            Assert.Null(ex);
        }

        private static void SetStatic(string fieldName, object value)
        {
            var f = typeof(SQLiteMembershipProvider).GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(f);
            f!.SetValue(null, value);
        }

        private static string GetStatic(string fieldName)
        {
            var f = typeof(SQLiteMembershipProvider).GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(f);
            return (string)f!.GetValue(null);
        }
    }
}
