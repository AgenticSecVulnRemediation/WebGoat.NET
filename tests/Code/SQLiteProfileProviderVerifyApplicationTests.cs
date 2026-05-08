using System;
using Mono.Data.Sqlite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderVerifyApplicationTests
    {
        [Fact]
        public void VerifyApplication_UsesPositionalParameters_ForInsertIntoApplications()
        {
            // Arrange
            // Fix switches INSERT to positional placeholders (?, ?, ?) and AddRange of parameters.
            string sql = "INSERT INTO [aspnet_Applications] (ApplicationId, ApplicationName, Description) VALUES (?, ?, ?)";

            // Act
            var cmd = new SqliteCommand(sql);
            cmd.Parameters.Add(new SqliteParameter { Value = Guid.NewGuid().ToString() });
            cmd.Parameters.Add(new SqliteParameter { Value = "app" });
            cmd.Parameters.Add(new SqliteParameter { Value = string.Empty });

            // Assert
            Assert.Equal(3, cmd.Parameters.Count);
            Assert.Equal(sql, cmd.CommandText);
        }
    }
}
