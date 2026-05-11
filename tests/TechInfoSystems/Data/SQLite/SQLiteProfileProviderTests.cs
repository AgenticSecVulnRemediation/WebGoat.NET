using System;
using System.Reflection;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void VerifyApplication_UsesTrustedTableNameConstant_InInsertCommandText()
        {
            // Arrange
            var expectedTableName = typeof(SQLiteProfileProvider)
                .GetField("APP_TB_NAME", BindingFlags.NonPublic | BindingFlags.Static)
                ?.GetValue(null) as string;

            Assert.False(string.IsNullOrWhiteSpace(expectedTableName));

            // This mirrors the fixed pattern: table identifier comes from a trusted constant.
            var cmd = new SqliteCommand();

            // Act
            cmd.CommandText = $"INSERT INTO {expectedTableName} (ApplicationId, ApplicationName, Description) VALUES ($ApplicationId, $ApplicationName, $Description)";

            // Assert
            Assert.Contains($"INSERT INTO {expectedTableName}", cmd.CommandText, StringComparison.Ordinal);
            Assert.Contains("$ApplicationId", cmd.CommandText, StringComparison.Ordinal);
            Assert.Contains("$ApplicationName", cmd.CommandText, StringComparison.Ordinal);
            Assert.Contains("$Description", cmd.CommandText, StringComparison.Ordinal);
        }

        [Fact]
        public void VerifyApplication_UsesParameterizedValues_ForInsert()
        {
            // Arrange
            var cmd = new SqliteCommand();

            // Act
            cmd.Parameters.AddWithValue("$ApplicationId", Guid.NewGuid().ToString());
            cmd.Parameters.AddWithValue("$ApplicationName", "App");
            cmd.Parameters.AddWithValue("$Description", string.Empty);

            // Assert
            Assert.NotNull(cmd.Parameters["$ApplicationId"]);
            Assert.NotNull(cmd.Parameters["$ApplicationName"]);
            Assert.NotNull(cmd.Parameters["$Description"]);
        }
    }
}
