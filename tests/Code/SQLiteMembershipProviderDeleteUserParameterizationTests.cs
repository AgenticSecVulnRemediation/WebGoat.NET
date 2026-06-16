using System;
using System.Data;
using System.Reflection;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserParameterizationTests
    {
        // NOTE: This test uses reflection to invoke the private static GetDBConnectionForMembership()
        // and to set private static fields required for DeleteUser() to execute.

        [Fact]
        public void DeleteUser_DeleteAllRelatedDataFalse_UsesAtNamedParameters_ForDeleteStatement()
        {
            // Arrange
            var provider = new SQLiteMembershipProvider();

            // Configure provider private static fields.
            SetStaticField("_connectionString", "Data Source=:memory:;Version=3;New=True;");
            SetStaticField("_applicationId", "app1");

            using var cn = CreateMembershipConnection();
            cn.Open();

            // Minimal schema required by DeleteUser when deleteAllRelatedData == false
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
CREATE TABLE [aspnet_Users] (
    UserId TEXT,
    Username TEXT,
    LoweredUsername TEXT,
    ApplicationId TEXT
);
";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO [aspnet_Users] (UserId, Username, LoweredUsername, ApplicationId) VALUES ('u1','Bob','bob','app1')";
                cmd.ExecuteNonQuery();
            }

            // Act
            var result = provider.DeleteUser("Bob", deleteAllRelatedData: false);

            // Assert
            Assert.True(result);

            // Ensure the user is deleted (sanity) and the call didn't throw due to parameter binding mismatch.
            using (var verify = cn.CreateCommand())
            {
                verify.CommandText = "SELECT COUNT(*) FROM [aspnet_Users] WHERE LoweredUsername='bob' AND ApplicationId='app1'";
                var remaining = Convert.ToInt32(verify.ExecuteScalar());
                Assert.Equal(0, remaining);
            }
        }

        [Fact]
        public void DeleteUser_DeleteAllRelatedDataTrue_StillResolvesSelectParameters_WhenMixingDollarAndAtPlaceholders()
        {
            // Arrange
            var provider = new SQLiteMembershipProvider();

            SetStaticField("_connectionString", "Data Source=:memory:;Version=3;New=True;");
            SetStaticField("_applicationId", "app1");

            using var cn = CreateMembershipConnection();
            cn.Open();

            // Schema required when deleteAllRelatedData == true (select UserId + delete relationship/profile tables)
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
CREATE TABLE [aspnet_Users] (
    UserId TEXT,
    Username TEXT,
    LoweredUsername TEXT,
    ApplicationId TEXT
);
CREATE TABLE [aspnet_UsersInRoles] (
    UserId TEXT
);
CREATE TABLE [aspnet_Profile] (
    UserId TEXT
);
";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO [aspnet_Users] (UserId, Username, LoweredUsername, ApplicationId) VALUES ('u1','Bob','bob','app1')";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO [aspnet_UsersInRoles] (UserId) VALUES ('u1')";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO [aspnet_Profile] (UserId) VALUES ('u1')";
                cmd.ExecuteNonQuery();
            }

            // Act
            var result = provider.DeleteUser("Bob", deleteAllRelatedData: true);

            // Assert
            Assert.True(result);

            using (var verify = cn.CreateCommand())
            {
                verify.CommandText = "SELECT COUNT(*) FROM [aspnet_Users]";
                Assert.Equal(0, Convert.ToInt32(verify.ExecuteScalar()));

                verify.CommandText = "SELECT COUNT(*) FROM [aspnet_UsersInRoles]";
                Assert.Equal(0, Convert.ToInt32(verify.ExecuteScalar()));

                verify.CommandText = "SELECT COUNT(*) FROM [aspnet_Profile]";
                Assert.Equal(0, Convert.ToInt32(verify.ExecuteScalar()));
            }
        }

        private static void SetStaticField(string fieldName, object value)
        {
            var field = typeof(SQLiteMembershipProvider).GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic);
            Assert.NotNull(field);
            field!.SetValue(null, value);
        }

        private static SqliteConnection CreateMembershipConnection()
        {
            var method = typeof(SQLiteMembershipProvider).GetMethod("GetDBConnectionForMembership", BindingFlags.Static | BindingFlags.NonPublic);
            Assert.NotNull(method);
            var cn = method!.Invoke(null, null) as SqliteConnection;
            Assert.NotNull(cn);
            return cn!;
        }
    }
}
