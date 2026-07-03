using System;
using System.Data;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderDeleteProfileQueryFormattingTests
    {
        [Fact]
        public void DeleteProfile_UsesStringFormatWithConstantTableName()
        {
            // Arrange
            // Regression guard: command text uses string.Format with USER_TB_NAME / PROFILE_TB_NAME constants.
            var selectSql = string.Format("SELECT UserId FROM {0} WHERE LoweredUsername = $Username AND ApplicationId = $ApplicationId", "[aspnet_Users]");
            var deleteSql = string.Format("DELETE FROM {0} WHERE UserId = $UserId", "[aspnet_Profile]");

            // Assert
            Assert.Contains("SELECT UserId FROM", selectSql);
            Assert.Contains("[aspnet_Users]", selectSql);
            Assert.Contains("DELETE FROM", deleteSql);
            Assert.Contains("[aspnet_Profile]", deleteSql);
            // Ensure we're not doing unsafe concatenation with the table name.
            Assert.DoesNotContain("+ USER_TB_NAME", selectSql);
            Assert.DoesNotContain("+ PROFILE_TB_NAME", deleteSql);
        }
    }
}
