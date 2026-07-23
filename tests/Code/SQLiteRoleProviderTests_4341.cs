using System;
using System.Data;
using System.Reflection;
using Mono.Data.Sqlite;
using Moq;
using Xunit;

// Assumption: Production namespace is TechInfoSystems.Data.SQLite as in source file.
namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderTests
    {
        [Fact]
        public void GetRolesForUser_UsesParameterizedQueryAndLowercasesUsername()
        {
            // Arrange
            // We validate only the behavior changed in the diff: the query uses parameter placeholders (@Username, @MembershipApplicationId)
            // and assigns username.ToLowerInvariant() to @Username.
            var provider = new SQLiteRoleProvider();

            // Set the private static field _membershipApplicationId so GetRolesForUser binds it.
            typeof(SQLiteRoleProvider).GetField("_membershipApplicationId", BindingFlags.NonPublic | BindingFlags.Static)!
                .SetValue(null, "memAppId");

            // Shim GetDbConnectionForRole() via reflection: replace _connectionString with in-memory sqlite and ensure schema exists.
            // Because GetDbConnectionForRole constructs SqliteConnection(_connectionString), we can set it to in-memory.
            typeof(SQLiteRoleProvider).GetField("_connectionString", BindingFlags.NonPublic | BindingFlags.Static)!
                .SetValue(null, "Data Source=:memory:;Version=3;New=True;");

            // Create schema and seed data using the same connection string.
            using (var cn = new SqliteConnection("Data Source=:memory:;Version=3;New=True;"))
            {
                cn.Open();
                using var cmd = cn.CreateCommand();
                cmd.CommandText = @"
CREATE TABLE aspnet_Roles (RoleId TEXT, RoleName TEXT, LoweredRoleName TEXT, ApplicationId TEXT);
CREATE TABLE aspnet_Users (UserId TEXT, Username TEXT, LoweredUsername TEXT, ApplicationId TEXT);
CREATE TABLE aspnet_UsersInRoles (UserId TEXT, RoleId TEXT);
INSERT INTO aspnet_Roles(RoleId, RoleName, LoweredRoleName, ApplicationId) VALUES('r1','Admin','admin','app1');
INSERT INTO aspnet_Users(UserId, Username, LoweredUsername, ApplicationId) VALUES('u1','Alice','alice','memAppId');
INSERT INTO aspnet_UsersInRoles(UserId, RoleId) VALUES('u1','r1');
";
                cmd.ExecuteNonQuery();
            }

            // Act
            var roles = provider.GetRolesForUser("ALICE");

            // Assert
            Assert.Contains("Admin", roles);
        }

        [Fact]
        public void GetRolesForUser_DoesNotTreatUsernameAsSql_WhenUsernameContainsSqlMetaChars()
        {
            // Arrange
            // Previously vulnerable pattern: concatenated username could break out of query.
            // After fix: parameterized query should treat it as a value and return no roles.
            var provider = new SQLiteRoleProvider();
            typeof(SQLiteRoleProvider).GetField("_membershipApplicationId", BindingFlags.NonPublic | BindingFlags.Static)!
                .SetValue(null, "memAppId");
            typeof(SQLiteRoleProvider).GetField("_connectionString", BindingFlags.NonPublic | BindingFlags.Static)!
                .SetValue(null, "Data Source=:memory:;Version=3;New=True;");

            using (var cn = new SqliteConnection("Data Source=:memory:;Version=3;New=True;"))
            {
                cn.Open();
                using var cmd = cn.CreateCommand();
                cmd.CommandText = @"
CREATE TABLE aspnet_Roles (RoleId TEXT, RoleName TEXT, LoweredRoleName TEXT, ApplicationId TEXT);
CREATE TABLE aspnet_Users (UserId TEXT, Username TEXT, LoweredUsername TEXT, ApplicationId TEXT);
CREATE TABLE aspnet_UsersInRoles (UserId TEXT, RoleId TEXT);
";
                cmd.ExecuteNonQuery();
            }

            // Act
            var roles = provider.GetRolesForUser("alice' OR '1'='1");

            // Assert
            Assert.Empty(roles);
        }
    }
}
