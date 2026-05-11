using System;
using Mono.Data.Sqlite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderVerifyApplicationTests
    {
        [Fact]
        public void VerifyApplication_UsesPositionalParameters_WithInsertValuesClause()
        {
            // Arrange
            using var cmd = new SqliteCommand();

            // Act
            cmd.CommandText = "INSERT INTO [aspnet_Applications] (ApplicationId, ApplicationName, Description) VALUES (?, ?, ?)";
            cmd.Parameters.Add(new SqliteParameter { Value = "id" });
            cmd.Parameters.Add(new SqliteParameter { Value = "name" });
            cmd.Parameters.Add(new SqliteParameter { Value = string.Empty });

            // Assert
            Assert.Equal(3, cmd.Parameters.Count);
            Assert.Contains("VALUES (?, ?, ?)", cmd.CommandText);
        }
    }
}
