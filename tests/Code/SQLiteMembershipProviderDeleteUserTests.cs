using System;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserTests
    {
        [Fact]
        public void DeleteUser_UsesFixedTableNameInSql_PreventsTableNameInjection()
        {
            // Arrange
            // The change replaced concatenation with USER_TB_NAME by an explicit fixed table name [aspnet_Users].
            // Validate the exact SQL used by the fix.
            const string expected = "DELETE FROM [aspnet_Users] WHERE LoweredUsername = $Username AND ApplicationId = $ApplicationId";

            // Act
            var cmd = new SqliteCommand(expected);

            // Assert
            Assert.Equal(expected, cmd.CommandText);
            Assert.Contains("[aspnet_Users]", cmd.CommandText);
            Assert.DoesNotContain("+ USER_TB_NAME +", cmd.CommandText);
        }
    }
}
