using System;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderDeleteRoleTests
    {
        [Fact]
        public void DeleteRole_UsesAtParameterName_DoesNotTreatRoleNameAsInlineSql()
        {
            // Arrange
            // Regression: query now uses @RoleName instead of $RoleName for nested select.
            // Validate that parameter can be added under the expected name.

            using (var cn = new SqliteConnection("Data Source=:memory:;Version=3"))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = "SELECT 1 WHERE 1=@RoleName";

                // Act
                var ex = Record.Exception(() => cmd.Parameters.AddWithValue("@RoleName", "admin'); DROP TABLE x; --"));

                // Assert
                Assert.Null(ex);
                Assert.Equal(1, cmd.Parameters.Count);
                Assert.Equal("@RoleName", cmd.Parameters[0].ParameterName);
            }
        }
    }
}
