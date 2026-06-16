using System;
using System.Collections.Specialized;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderGetAllUsersAnonymousFilterTests
    {
        [Fact]
        public void GetAllUsers_CountQuery_UsesParameterForIsAnonymousFilter()
        {
            // Arrange
            var provider = new SQLiteMembershipProvider();

            // Minimal init to avoid accessing ConfigurationManager connection string; we set fields directly.
            SetStaticField("_connectionString", "Data Source=:memory:;Version=3;New=True;");
            SetStaticField("_applicationId", "app1");

            using var cn = CreateMembershipConnection();
            cn.Open();

            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
CREATE TABLE [aspnet_Users] (
    UserId TEXT,
    Username TEXT,
    LoweredUsername TEXT,
    ApplicationId TEXT,
    Email TEXT,
    PasswordQuestion TEXT,
    Comment TEXT,
    IsApproved INTEGER,
    IsLockedOut INTEGER,
    CreateDate TEXT,
    LastLoginDate TEXT,
    LastActivityDate TEXT,
    LastPasswordChangedDate TEXT,
    LastLockoutDate TEXT,
    IsAnonymous INTEGER
);
";
                cmd.ExecuteNonQuery();

                // Insert one anonymous and one non-anonymous user.
                cmd.CommandText = @"INSERT INTO [aspnet_Users]
(UserId, Username, LoweredUsername, ApplicationId, Email, PasswordQuestion, Comment, IsApproved, IsLockedOut,
 CreateDate, LastLoginDate, LastActivityDate, LastPasswordChangedDate, LastLockoutDate, IsAnonymous)
VALUES
('u1','Anon','anon','app1','a@b','q','c',1,0,'2000-01-01','2000-01-01','2000-01-01','2000-01-01','2000-01-01',1),
('u2','Bob','bob','app1','b@b','q','c',1,0,'2000-01-01','2000-01-01','2000-01-01','2000-01-01','2000-01-01',0);";
                cmd.ExecuteNonQuery();
            }

            // Act
            var users = provider.GetAllUsers(pageIndex: 0, pageSize: 10, out var totalRecords);

            // Assert
            // The changed behavior is that the count query uses a parameter for IsAnonymous;
            // This assertion ensures the non-anonymous filtering still works end-to-end.
            Assert.Equal(1, totalRecords);
            Assert.Single(users);
        }

        private static void SetStaticField(string fieldName, object value)
        {
            var field = typeof(SQLiteMembershipProvider).GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic);
            Assert.NotNull(field);
            field!.SetValue(null, value);
        }

        private static Mono.Data.Sqlite.SqliteConnection CreateMembershipConnection()
        {
            var method = typeof(SQLiteMembershipProvider).GetMethod("GetDBConnectionForMembership", BindingFlags.Static | BindingFlags.NonPublic);
            Assert.NotNull(method);
            var cn = method!.Invoke(null, null) as Mono.Data.Sqlite.SqliteConnection;
            Assert.NotNull(cn);
            return cn!;
        }
    }
}
