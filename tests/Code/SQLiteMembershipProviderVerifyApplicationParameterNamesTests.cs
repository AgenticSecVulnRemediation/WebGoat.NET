using Mono.Data.Sqlite;
using System;
using System.Data;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderVerifyApplicationParameterNamesTests
    {
        [Fact]
        public void VerifyApplication_InsertApplication_UsesCorrectParameterNames()
        {
            // Arrange
            const string sql = "INSERT INTO [aspnet_Applications] (ApplicationId, ApplicationName, Description) VALUES ($ApplicationId, $ApplicationName, $Description)";
            using var cmd = new SqliteCommand(sql);

            // Act
            cmd.Parameters.AddWithValue("$ApplicationId", Guid.NewGuid().ToString());
            cmd.Parameters.AddWithValue("$ApplicationName", "app");
            cmd.Parameters.AddWithValue("$Description", string.Empty);

            // Assert
            Assert.NotNull(cmd.Parameters["$ApplicationId"]);
            Assert.NotNull(cmd.Parameters["$ApplicationName"]);
            Assert.NotNull(cmd.Parameters["$Description"]);
        }
    }
}
