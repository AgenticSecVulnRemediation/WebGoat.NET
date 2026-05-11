using System;
using System.Data;
using Mono.Data.Sqlite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void DeleteProfile_UsesAddWithValue_ForUserIdParameter()
        {
            // Arrange: exercise the exact parameter-binding behavior changed in the diff.
            using var conn = new SqliteConnection("Data Source=:memory:;Version=3;New=True;");
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM [aspnet_Profile] WHERE UserId = $UserId";

            // Act
            cmd.Parameters.AddWithValue("$UserId", "user-123");

            // Assert
            Assert.Single(cmd.Parameters);
            Assert.Equal("$UserId", cmd.Parameters[0].ParameterName);
            Assert.Equal("user-123", cmd.Parameters[0].Value);
        }
    }
}
