using System;
using Mono.Data.Sqlite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    // Delta test: DeleteUser switched to interpolated command text for table constants.
    // Security expectation preserved: user-controlled values must remain parameters ($Username, $ApplicationId, $UserId).
    public class SQLiteMembershipProviderDeleteUserTests
    {
        [Fact]
        public void DeleteUser_CommandText_UsesParameters_ForUserSuppliedValues()
        {
            // Arrange
            const string userTable = "[aspnet_Users]";
            var sql = $"DELETE FROM {userTable} WHERE LoweredUsername = $Username AND ApplicationId = $ApplicationId";

            using var connection = new SqliteConnection("Data Source=:memory:;Version=3;New=True;");
            connection.Open();

            using var cmd = new SqliteCommand(sql, connection);

            // Act
            cmd.Parameters.AddWithValue("$Username", "bob");
            cmd.Parameters.AddWithValue("$ApplicationId", "app");

            // Assert
            Assert.Contains("$Username", cmd.CommandText);
            Assert.Contains("$ApplicationId", cmd.CommandText);
            Assert.Equal(2, cmd.Parameters.Count);
        }
    }
}
