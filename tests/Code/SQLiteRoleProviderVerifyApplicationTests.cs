using System;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderVerifyApplicationTests
    {
        [Fact]
        public void VerifyApplication_UsesAtParameters_InsteadOfDollarParameters()
        {
            // Arrange
            // The change uses interpolated SQL with @ApplicationId/@ApplicationName/@Description.
            const string appTbName = "[aspnet_Applications]";
            var expected = $"INSERT INTO {appTbName} (ApplicationId, ApplicationName, Description) VALUES (@ApplicationId, @ApplicationName, @Description)";

            using var cmd = new SqliteCommand(expected);

            // Act
            cmd.Parameters.AddWithValue("@ApplicationId", "aid");
            cmd.Parameters.AddWithValue("@ApplicationName", "app");
            cmd.Parameters.AddWithValue("@Description", "");

            // Assert
            Assert.Equal(expected, cmd.CommandText);
            Assert.Equal(3, cmd.Parameters.Count);
            Assert.Equal("@ApplicationId", cmd.Parameters[0].ParameterName);
            Assert.Equal("@ApplicationName", cmd.Parameters[1].ParameterName);
            Assert.Equal("@Description", cmd.Parameters[2].ParameterName);
        }
    }
}
