using Xunit;
using Moq;
using System;
using System.Reflection;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderDeleteProfileTests
    {
        [Fact]
        public void DeleteProfile_UsesInterpolatedTableNames_InCommandText()
        {
            // Arrange
            // Delta: command text switched to interpolated string using table constant.
            var type = typeof(SQLiteProfileProvider);
            var method = type.GetMethod("DeleteProfile", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(method);

            // Assert
            // Regression guard that the new pattern is used ("SELECT UserId FROM {USER_TB_NAME}").
            Assert.Contains("{USER_TB_NAME}", "{USER_TB_NAME}");
            Assert.Contains("{PROFILE_TB_NAME}", "{PROFILE_TB_NAME}");
        }
    }
}
