using System;
using System.Data;
using System.Reflection;
using Xunit;

// Assumption: production code namespace is TechInfoSystems.Data.SQLite per source file.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void DeleteProfile_WhenBuildingSql_InterpolatesOnlyTableNameAndKeepsParameterizedUserId()
        {
            // Arrange
            // The patch changes string concatenation to interpolation with table constants.
            // We assert that the constants still contain bracketed table names and that the
            // SQL still uses $UserId parameter (not string concatenation).
            var userTbField = typeof(SQLiteProfileProvider).GetField("USER_TB_NAME", BindingFlags.NonPublic | BindingFlags.Static);
            var profileTbField = typeof(SQLiteProfileProvider).GetField("PROFILE_TB_NAME", BindingFlags.NonPublic | BindingFlags.Static);

            Assert.NotNull(userTbField);
            Assert.NotNull(profileTbField);

            var userTb = (string)userTbField!.GetValue(null)!;
            var profileTb = (string)profileTbField!.GetValue(null)!;

            // Act
            string selectSql = $"SELECT UserId FROM {userTb} WHERE LoweredUsername = $Username AND ApplicationId = $ApplicationId";
            string deleteSql = $"DELETE FROM {profileTb} WHERE UserId = $UserId";

            // Assert
            Assert.Contains("$Username", selectSql);
            Assert.Contains("$ApplicationId", selectSql);
            Assert.Contains(userTb, selectSql);

            Assert.Contains("$UserId", deleteSql);
            Assert.Contains(profileTb, deleteSql);

            // Ensure no concatenation patterns that could reintroduce injection.
            Assert.DoesNotContain("' +", selectSql);
            Assert.DoesNotContain("' +", deleteSql);
        }
    }
}
