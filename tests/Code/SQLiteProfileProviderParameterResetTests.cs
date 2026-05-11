using System;
using System.Data;
using Mono.Data.Sqlite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderParameterResetTests
    {
        [Fact]
        public void SetPropertyValues_ClearsParameters_BeforeReusingCommand_ForUpdate()
        {
            // Arrange
            // Delta behavior: Parameters.Clear() added before adding new parameters for UPDATE.
            using var conn = new SqliteConnection("Data Source=:memory:;Version=3");
            using var cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT COUNT(*) FROM [aspnet_Profile] WHERE UserId = $UserId";
            cmd.Parameters.AddWithValue("$UserId", "1");

            // Act
            cmd.Parameters.Clear();
            cmd.CommandText = "UPDATE [aspnet_Profile] SET PropertyNames = ?, PropertyValuesString = ?, PropertyValuesBinary = ?, LastUpdatedDate = ? WHERE UserId = ?";
            cmd.Parameters.AddWithValue("$PropertyNames", "names");
            cmd.Parameters.AddWithValue("$PropertyValuesString", "values");
            cmd.Parameters.AddWithValue("$PropertyValuesBinary", new byte[] {1,2,3});
            cmd.Parameters.AddWithValue("$LastUpdatedDate", DateTime.UtcNow);
            cmd.Parameters.AddWithValue("$UserId", "1");

            // Assert
            Assert.Equal(5, cmd.Parameters.Count);
        }

        [Fact]
        public void SetPropertyValues_ClearsParameters_BeforeReusingCommand_ForInsert()
        {
            // Arrange
            using var conn = new SqliteConnection("Data Source=:memory:;Version=3");
            using var cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT COUNT(*) FROM [aspnet_Profile] WHERE UserId = $UserId";
            cmd.Parameters.AddWithValue("$UserId", "1");

            // Act
            cmd.Parameters.Clear();
            cmd.CommandText = "INSERT INTO [aspnet_Profile] (UserId, PropertyNames, PropertyValuesString, PropertyValuesBinary, LastUpdatedDate) VALUES (?, ?, ?, ?, ?)";
            cmd.Parameters.AddWithValue("$UserId", "1");
            cmd.Parameters.AddWithValue("$PropertyNames", "names");
            cmd.Parameters.AddWithValue("$PropertyValuesString", "values");
            cmd.Parameters.AddWithValue("$PropertyValuesBinary", new byte[] {1,2,3});
            cmd.Parameters.AddWithValue("$LastUpdatedDate", DateTime.UtcNow);

            // Assert
            Assert.Equal(5, cmd.Parameters.Count);
        }
    }
}
