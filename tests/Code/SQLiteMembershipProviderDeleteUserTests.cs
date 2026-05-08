using System;
using System.Text.RegularExpressions;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserTests
    {
        [Fact]
        public void DeleteUser_ClearsParameters_BeforeReuseToAvoidParameterMixup()
        {
            // Arrange
            // Fix adds cmd.Parameters.Clear() before reusing command for DELETE to avoid parameter collisions.
            var cmd = new Mono.Data.Sqlite.SqliteCommand();
            cmd.CommandText = "SELECT UserId FROM [aspnet_Users] WHERE LoweredUsername = $Username AND ApplicationId = $ApplicationId";
            cmd.Parameters.AddWithValue("$Username", "user");
            cmd.Parameters.AddWithValue("$ApplicationId", "app");

            // Act
            cmd.Parameters.Clear();
            cmd.CommandText = "DELETE FROM [aspnet_Users] WHERE LoweredUsername = $Username AND ApplicationId = $ApplicationId";
            cmd.Parameters.AddWithValue("$Username", "user");
            cmd.Parameters.AddWithValue("$ApplicationId", "app");

            // Assert
            Assert.Equal(2, cmd.Parameters.Count);
            Assert.NotNull(cmd.Parameters["$Username"]);
            Assert.NotNull(cmd.Parameters["$ApplicationId"]);
        }
    }
}
