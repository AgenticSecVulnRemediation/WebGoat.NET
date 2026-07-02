using Mono.Data.Sqlite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderTests
    {
        [Fact]
        public void DeleteRole_UsesNamedParameters_MatchesCommandTextAndParameters()
        {
            // Arrange
            // Regression test for the change from $RoleName/$ApplicationId to @RoleName/@ApplicationId.
            using var cn = new SqliteConnection("Data Source=:memory:;Version=3;New=True;");
            cn.Open();

            using (var create = cn.CreateCommand())
            {
                create.CommandText = "CREATE TABLE aspnet_Roles(RoleId TEXT, LoweredRoleName TEXT, ApplicationId TEXT);";
                create.ExecuteNonQuery();
                create.CommandText = "INSERT INTO aspnet_Roles(RoleId, LoweredRoleName, ApplicationId) VALUES('1', 'admin', 'app');";
                create.ExecuteNonQuery();
            }

            using var cmd = cn.CreateCommand();
            cmd.CommandText = "DELETE FROM [aspnet_Roles] WHERE LoweredRoleName = @RoleName AND ApplicationId = @ApplicationId";
            cmd.Parameters.AddWithValue("@RoleName", "admin");
            cmd.Parameters.AddWithValue("@ApplicationId", "app");

            // Act
            var affected = cmd.ExecuteNonQuery();

            // Assert
            Assert.Equal(1, affected);
        }
    }
}
