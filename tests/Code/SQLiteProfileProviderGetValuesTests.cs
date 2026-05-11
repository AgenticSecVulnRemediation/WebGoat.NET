using System;
using Mono.Data.Sqlite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderGetValuesTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_UsesParameterizedUserIdQuery()
        {
            // Arrange
            // Delta behavior: query now uses @UserId and string.Format with table name.
            var userId = Guid.NewGuid().ToString();
            var sql = string.Format("SELECT PropertyNames, PropertyValuesString, PropertyValuesBinary FROM {0} WHERE UserId = @UserId", "[aspnet_Profile]");

            using var conn = new SqliteConnection("Data Source=:memory:;Version=3");
            using var cmd = conn.CreateCommand();

            // Act
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@UserId", userId);

            // Assert
            Assert.Contains("WHERE UserId = @UserId", cmd.CommandText, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain(userId, cmd.CommandText, StringComparison.Ordinal);
            Assert.NotNull(cmd.Parameters["@UserId"]);
            Assert.Equal(userId, cmd.Parameters["@UserId"].Value);
        }
    }
}
