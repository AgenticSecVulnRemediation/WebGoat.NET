using System;
using Xunit;
using Mono.Data.Sqlite;

// Assumption: source namespace is TechInfoSystems.Data.SQLite (from file content)
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void DeleteUser_RelatedDeletes_UseAtUserIdParameterMarker()
        {
            // Arrange
            var usersInRolesSql = "DELETE FROM [aspnet_UsersInRoles] WHERE UserId = @UserId";
            var profileSql = "DELETE FROM [aspnet_Profile] WHERE UserId = @UserId";

            using var cmd = new SqliteCommand();

            // Act
            cmd.CommandText = usersInRolesSql;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@UserId", "user-id");

            // Assert
            Assert.Contains("WHERE UserId = @UserId", cmd.CommandText);
            Assert.DoesNotContain("WHERE UserId = $UserId", cmd.CommandText);
            Assert.NotNull(cmd.Parameters["@UserId"]);

            // Act (second command text)
            cmd.CommandText = profileSql;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@UserId", "user-id");

            // Assert
            Assert.Contains("WHERE UserId = @UserId", cmd.CommandText);
            Assert.DoesNotContain("WHERE UserId = $UserId", cmd.CommandText);
            Assert.NotNull(cmd.Parameters["@UserId"]);
        }
    }
}
