using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderDeleteProfileQueryFormattingTests
    {
        [Fact]
        public void DeleteProfile_UsesConstantTableNames_NotUserControlledInput()
        {
            // Arrange/Act
            // The change refactors concatenation into string.Format with constant table names.
            var selectSql = string.Format("SELECT UserId FROM {0} WHERE LoweredUsername = $Username AND ApplicationId = $ApplicationId", "[aspnet_Users]");
            var deleteSql = string.Format("DELETE FROM {0} WHERE UserId = $UserId", "[aspnet_Profile]");

            // Assert
            Assert.Contains("[aspnet_Users]", selectSql);
            Assert.Contains("[aspnet_Profile]", deleteSql);
            Assert.DoesNotContain("+", selectSql); // no concatenation expected in the formatted string
            Assert.DoesNotContain("+", deleteSql);
        }
    }
}
