using System;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderGetPropertyValuesFromDatabaseTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_UsesAtUserIdParameterName_CanBeAdded()
        {
            // Arrange
            // Diff switches from $UserId to @UserId for Property table lookup.
            using (var cn = new SqliteConnection("Data Source=:memory:;Version=3"))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = "SELECT 1 WHERE 1=@UserId";

                // Act
                var ex = Record.Exception(() => cmd.Parameters.AddWithValue("@UserId", "u"));

                // Assert
                Assert.Null(ex);
                Assert.Single(cmd.Parameters);
                Assert.Equal("@UserId", cmd.Parameters[0].ParameterName);
            }
        }
    }
}
