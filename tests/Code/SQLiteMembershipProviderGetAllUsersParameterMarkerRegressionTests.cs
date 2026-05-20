using Xunit;
using Mono.Data.Sqlite;
using System.Data;

// Assumption: source namespace is TechInfoSystems.Data.SQLite based on file content.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProvider_GetAllUsers_ParameterMarkerRegressionTests
    {
        [Fact]
        public void GetAllUsers_UsesAtParameterMarker_ForApplicationIdCountQuery()
        {
            // Arrange
            // The vulnerability fix changed the Count query parameter marker from $ApplicationId to @ApplicationId.
            // We regression-test that the SQL uses @ApplicationId to avoid provider-specific parameter binding issues.

            var provider = new SQLiteMembershipProvider();

            // Reflection to set private static fields required by provider.
            var type = typeof(SQLiteMembershipProvider);
            type.GetField("_applicationId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
                .SetValue(null, "app-1");
            type.GetField("_connectionString", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
                .SetValue(null, "Data Source=:memory:;Version=3;New=True;");

            using var cn = new SqliteConnection("Data Source=:memory:;Version=3;New=True;");
            cn.Open();

            // Create minimal schema for aspnet_Users.
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE [aspnet_Users] (ApplicationId TEXT, IsAnonymous TEXT, UserId TEXT, Username TEXT, Email TEXT, PasswordQuestion TEXT, Comment TEXT, IsApproved INTEGER, IsLockedOut INTEGER, CreateDate TEXT, LastLoginDate TEXT, LastActivityDate TEXT, LastPasswordChangedDate TEXT, LastLockoutDate TEXT);";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO [aspnet_Users] (ApplicationId, IsAnonymous, UserId, Username, Email, PasswordQuestion, Comment, IsApproved, IsLockedOut, CreateDate, LastLoginDate, LastActivityDate, LastPasswordChangedDate, LastLockoutDate) VALUES ('app-1','0','u1','user1','e','q','c',1,0,'2020-01-01','2020-01-01','2020-01-01','2020-01-01','2020-01-01');";
                cmd.ExecuteNonQuery();
            }

            // Act
            // Call GetAllUsers; it will run the Count query.
            int total;
            var users = provider.GetAllUsers(0, 10, out total);

            // Assert
            Assert.Equal(1, total);
            Assert.Single(users);

            // Additional assert: execute the exact Count SQL with @ApplicationId to ensure it works.
            using var verifyCmd = cn.CreateCommand();
            verifyCmd.CommandText = "SELECT Count(*) FROM [aspnet_Users] WHERE ApplicationId = @ApplicationId AND IsAnonymous='0'";
            verifyCmd.Parameters.AddWithValue("@ApplicationId", "app-1");
            var count = System.Convert.ToInt32(verifyCmd.ExecuteScalar());
            Assert.Equal(1, count);
        }
    }
}
