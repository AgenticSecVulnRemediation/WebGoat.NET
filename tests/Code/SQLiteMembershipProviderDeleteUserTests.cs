using Xunit;
using System;
using System.Data;
using System.Reflection;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserTests
    {
        [Fact]
        public void DeleteUser_DeleteAllRelatedData_DeletesUserAndRelatedRows()
        {
            // Arrange: Build an in-memory DB with minimal schema and rows.
            var provider = new SQLiteMembershipProvider();

            var cs = "Data Source=:memory:;Version=3;New=True;";
            var appId = Guid.NewGuid().ToString();

            SetStaticField(typeof(SQLiteMembershipProvider), "_connectionString", cs);
            SetStaticField(typeof(SQLiteMembershipProvider), "_applicationId", appId);

            using (var cn = new SqliteConnection(cs))
            {
                cn.Open();
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = @"
CREATE TABLE [aspnet_Users] (UserId TEXT PRIMARY KEY, LoweredUsername TEXT, ApplicationId TEXT);
CREATE TABLE [aspnet_UsersInRoles] (UserId TEXT);
CREATE TABLE [aspnet_Profile] (UserId TEXT);
";
                    cmd.ExecuteNonQuery();

                    var userId = Guid.NewGuid().ToString();
                    cmd.CommandText = "INSERT INTO [aspnet_Users](UserId, LoweredUsername, ApplicationId) VALUES ($UserId,$Username,$AppId)";
                    cmd.Parameters.AddWithValue("$UserId", userId);
                    cmd.Parameters.AddWithValue("$Username", "someuser");
                    cmd.Parameters.AddWithValue("$AppId", appId);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    cmd.CommandText = "INSERT INTO [aspnet_UsersInRoles](UserId) VALUES ($UserId)";
                    cmd.Parameters.AddWithValue("$UserId", userId);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    cmd.CommandText = "INSERT INTO [aspnet_Profile](UserId) VALUES ($UserId)";
                    cmd.Parameters.AddWithValue("$UserId", userId);
                    cmd.ExecuteNonQuery();
                }

                // Act
                var deleted = provider.DeleteUser("SomeUser", deleteAllRelatedData: true);

                // Assert
                Assert.True(deleted);

                using (var verify = cn.CreateCommand())
                {
                    verify.CommandText = "SELECT COUNT(*) FROM [aspnet_Users]";
                    Assert.Equal(0L, (long)verify.ExecuteScalar());
                    verify.CommandText = "SELECT COUNT(*) FROM [aspnet_UsersInRoles]";
                    Assert.Equal(0L, (long)verify.ExecuteScalar());
                    verify.CommandText = "SELECT COUNT(*) FROM [aspnet_Profile]";
                    Assert.Equal(0L, (long)verify.ExecuteScalar());
                }
            }
        }

        private static void SetStaticField(Type t, string fieldName, object value)
        {
            var f = t.GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic);
            Assert.NotNull(f);
            f!.SetValue(null, value);
        }
    }
}
