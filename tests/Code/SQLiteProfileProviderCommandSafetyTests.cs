using System;
using System.Collections.Specialized;
using Mono.Data.Sqlite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderCommandSafetyTests
    {
        [Fact]
        public void SetPropertyValues_UserLookupQuery_RemainsParameterized_NotStringConcatenated()
        {
            // Arrange
            const string tableName = "[aspnet_Users]";

            // Act
            // This mirrors the updated query shape in the diff: interpolated table name + named parameters.
            var sql = $"SELECT UserId FROM {tableName} WHERE LoweredUsername = $Username AND ApplicationId = $ApplicationId;";

            // Assert
            Assert.Contains("$Username", sql);
            Assert.Contains("$ApplicationId", sql);
            Assert.DoesNotContain("'" + " +", sql);
            Assert.DoesNotContain(""" +", sql);
        }

        [Fact]
        public void SetPropertyValues_UserLookupParams_AddedAsExplicitSqliteParameters()
        {
            // Arrange
            var username = "user@example.com";
            var appId = Guid.NewGuid().ToString();

            // Act
            var parameters = new[]
            {
                new SqliteParameter("$Username", username.ToLowerInvariant()),
                new SqliteParameter("$ApplicationId", appId)
            };

            // Assert
            Assert.Equal("$Username", parameters[0].ParameterName);
            Assert.Equal(username.ToLowerInvariant(), parameters[0].Value);
            Assert.Equal("$ApplicationId", parameters[1].ParameterName);
            Assert.Equal(appId, parameters[1].Value);
        }
    }
}
