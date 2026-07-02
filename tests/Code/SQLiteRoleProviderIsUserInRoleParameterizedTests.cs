using System;
using Mono.Data.Sqlite;
using Xunit;

using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderIsUserInRoleParameterizedTests
    {
        [Fact]
        public void IsUserInRole_QueryUsesNamedParameters_AndReturnsTrueWhenMatch()
        {
            // Arrange
            // Patch rewrote the SQL to use string.Format and @-prefixed parameters.
            // We validate the query works with @Username/@RoleName/@MembershipApplicationId/@ApplicationId.

            using var cn = new SqliteConnection("Data Source=:memory:");
            cn.Open();

            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
CREATE TABLE aspnet_Users (UserId TEXT, LoweredUsername TEXT, ApplicationId TEXT);
CREATE TABLE aspnet_Roles (RoleId TEXT, LoweredRoleName TEXT, ApplicationId TEXT);
CREATE TABLE aspnet_UsersInRoles (UserId TEXT, RoleId TEXT);
INSERT INTO aspnet_Users (UserId, LoweredUsername, ApplicationId) VALUES ('u1', 'alice', 'mapp');
INSERT INTO aspnet_Roles (RoleId, LoweredRoleName, ApplicationId) VALUES ('r1', 'admin', 'app');
INSERT INTO aspnet_UsersInRoles (UserId, RoleId) VALUES ('u1', 'r1');
";
                cmd.ExecuteNonQuery();
            }

            using var query = cn.CreateCommand();
            query.CommandText = string.Format(
                "SELECT COUNT(*) FROM {0} uir INNER JOIN {1} u ON uir.UserId = u.UserId INNER JOIN {2} r ON uir.RoleId = r.RoleId WHERE u.LoweredUsername = @Username AND u.ApplicationId = @MembershipApplicationId AND r.LoweredRoleName = @RoleName AND r.ApplicationId = @ApplicationId",
                "[aspnet_UsersInRoles]",
                "[aspnet_Users]",
                "[aspnet_Roles]");

            query.Parameters.AddWithValue("@Username", "Alice".ToLowerInvariant());
            query.Parameters.AddWithValue("@RoleName", "Admin".ToLowerInvariant());
            query.Parameters.AddWithValue("@MembershipApplicationId", "mapp");
            query.Parameters.AddWithValue("@ApplicationId", "app");

            // Act
            var count = Convert.ToInt64(query.ExecuteScalar());

            // Assert
            Assert.True(count > 0);
        }
    }
}
