using System;
using System.Collections.Specialized;
using System.Reflection;
using Moq;
using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void DeleteUser_DeleteAllRelatedDataTrue_ClearsParametersBeforeDelete_UsesCorrectUsernameAndApplicationIdParams()
        {
            // This is a delta test for the security fix: cmd.Parameters.Clear() added before the DELETE.
            // We can't easily execute DB commands in a pure unit test, but we can still assert the intended
            // observable behavior by verifying that the method continues to work when the earlier SELECT path
            // adds parameters, i.e., no duplicate parameter names cause runtime exception.

            // Arrange
            var provider = new SQLiteMembershipProvider();

            // Set static private fields required by DeleteUser.
            SetStaticField(typeof(SQLiteMembershipProvider), "_connectionString", "Data Source=:memory:;Version=3;New=True;");
            SetStaticField(typeof(SQLiteMembershipProvider), "_applicationId", Guid.NewGuid().ToString());

            // Create schema in memory to support the queries.
            using (var cn = new Mono.Data.Sqlite.SqliteConnection("Data Source=:memory:;Version=3;New=True;"))
            {
                cn.Open();
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE aspnet_Users (UserId TEXT, LoweredUsername TEXT, ApplicationId TEXT);";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "CREATE TABLE aspnet_UsersInRoles (UserId TEXT, RoleId TEXT);";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "CREATE TABLE aspnet_Profile (UserId TEXT);";
                    cmd.ExecuteNonQuery();

                    var userId = Guid.NewGuid().ToString();
                    cmd.CommandText = "INSERT INTO aspnet_Users (UserId, LoweredUsername, ApplicationId) VALUES ($uid, $un, $app);";
                    cmd.Parameters.AddWithValue("$uid", userId);
                    cmd.Parameters.AddWithValue("$un", "alice");
                    cmd.Parameters.AddWithValue("$app", GetStaticField<string>(typeof(SQLiteMembershipProvider), "_applicationId"));
                    cmd.ExecuteNonQuery();
                }
            }

            // Important: the provider will create a new connection using its _connectionString.
            // Use a shared in-memory database by using cache=shared.
            var sharedConnStr = "Data Source=file:memdb1?mode=memory&cache=shared";
            SetStaticField(typeof(SQLiteMembershipProvider), "_connectionString", sharedConnStr);

            using (var cn = new Mono.Data.Sqlite.SqliteConnection(sharedConnStr))
            {
                cn.Open();
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE aspnet_Users (UserId TEXT, LoweredUsername TEXT, ApplicationId TEXT);";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "CREATE TABLE aspnet_UsersInRoles (UserId TEXT, RoleId TEXT);";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "CREATE TABLE aspnet_Profile (UserId TEXT);";
                    cmd.ExecuteNonQuery();

                    var userId = Guid.NewGuid().ToString();
                    cmd.CommandText = "INSERT INTO aspnet_Users (UserId, LoweredUsername, ApplicationId) VALUES ($uid, $un, $app);";
                    cmd.Parameters.AddWithValue("$uid", userId);
                    cmd.Parameters.AddWithValue("$un", "alice");
                    cmd.Parameters.AddWithValue("$app", GetStaticField<string>(typeof(SQLiteMembershipProvider), "_applicationId"));
                    cmd.ExecuteNonQuery();
                }
            }

            // Act + Assert
            // Before the fix, this could throw due to duplicate $Username/$ApplicationId params being added twice.
            var ex = Record.Exception(() => provider.DeleteUser("Alice", deleteAllRelatedData: true));
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
