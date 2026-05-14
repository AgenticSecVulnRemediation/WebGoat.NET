using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Data.Sqlite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_UsesAtUserIdParameterMarker()
        {
            // Arrange
            // Delta: query uses "@UserId" instead of "$UserId".
            string sql = string.Format("SELECT PropertyNames, PropertyValuesString, PropertyValuesBinary FROM {0} WHERE UserId = @UserId", "[aspnet_Profile]");

            using var conn = new SqliteConnection("Data Source=:memory:;Version=3");
            conn.Open();

            using var cmd = new SqliteCommand(sql, conn);

            // Act
            cmd.Parameters.AddWithValue("@UserId", Guid.NewGuid().ToString());

            // Assert
            Assert.Contains("@UserId", cmd.CommandText);
            Assert.DoesNotContain("$UserId", cmd.CommandText);
            Assert.Single(cmd.Parameters);
            Assert.Equal("@UserId", cmd.Parameters[0].ParameterName);
        }
    }
}
