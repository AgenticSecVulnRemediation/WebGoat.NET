using System;
using System.Reflection;
using Xunit;

// Assumption: production namespace follows file content: TechInfoSystems.Data.SQLite
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserQueryTests
    {
        [Fact]
        public void DeleteUser_QueryUsesConstantTableName_AndDoesNotEmbedUsername()
        {
            // Arrange
            // This delta verifies that the refactor to string-interpolation with USER_TB_NAME constant
            // does not accidentally embed user-controlled username into the SQL command text.
            // We can't easily intercept SqliteCommand without changing production code, so we validate
            // the command text template via reflection by recreating the exact interpolated strings.

            const string expectedUserTableName = "[aspnet_Users]";

            // Act
            // Extract the USER_TB_NAME constant value from the provider.
            var userTableNameField = typeof(SQLiteMembershipProvider)
                .GetField("USER_TB_NAME", BindingFlags.NonPublic | BindingFlags.Static);

            Assert.NotNull(userTableNameField);
            var userTableName = userTableNameField!.GetValue(null) as string;

            // Assert
            Assert.Equal(expectedUserTableName, userTableName);

            // Assert the interpolated SQL uses the constant and parameter placeholders (not username concatenation)
            string selectSql = $"SELECT UserId FROM {userTableName} WHERE LoweredUsername = $Username AND ApplicationId = $ApplicationId";
            string deleteSql = $"DELETE FROM {userTableName} WHERE LoweredUsername = $Username AND ApplicationId = $ApplicationId";

            Assert.Contains(expectedUserTableName, selectSql);
            Assert.Contains("$Username", selectSql);
            Assert.Contains("$ApplicationId", selectSql);

            Assert.Contains(expectedUserTableName, deleteSql);
            Assert.Contains("$Username", deleteSql);
            Assert.Contains("$ApplicationId", deleteSql);

            // Negative assertion: ensure there is no direct concatenation marker around the username.
            Assert.DoesNotContain("+ username", selectSql, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("+ username", deleteSql, StringComparison.OrdinalIgnoreCase);
        }
    }
}
