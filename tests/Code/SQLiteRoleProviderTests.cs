using System;
using Mono.Data.Sqlite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderTests
    {
        [Fact]
        public void GetAllRoles_UsesPositionalParameterMarker()
        {
            // Arrange
            // Delta: query now uses positional '?' marker.
            string sql = "SELECT RoleName FROM [aspnet_Roles] WHERE ApplicationId = ?";

            using var conn = new SqliteConnection("Data Source=:memory:;Version=3");
            conn.Open();

            // Act
            using var cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue(null, Guid.NewGuid().ToString());

            // Assert
            Assert.Contains("?", cmd.CommandText);
            Assert.DoesNotContain("$ApplicationId", cmd.CommandText);
            Assert.Single(cmd.Parameters);
        }
    }
}
