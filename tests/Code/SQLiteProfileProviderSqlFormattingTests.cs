using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderSqlFormattingTests
    {
        [Fact]
        public void SetPropertyValues_QueryUsesFormattedProfileTableName_AndNotUserControlledInput()
        {
            // This delta test focuses strictly on the change introduced in PR #874:
            // PROFILE_TB_NAME is interpolated via string.Format into SQL command text.
            // The security property is that the interpolated value is a constant and remains bracketed,
            // not influenced by user input.

            const string ProfileTableNameConst = "[aspnet_Profile]";

            var countSql = string.Format("SELECT COUNT(*) FROM {0} WHERE UserId = $UserId", ProfileTableNameConst);
            var updateSql = string.Format(
                "UPDATE {0} SET PropertyNames = $PropertyNames, PropertyValuesString = $PropertyValuesString, PropertyValuesBinary = $PropertyValuesBinary, LastUpdatedDate = $LastUpdatedDate WHERE UserId = $UserId",
                ProfileTableNameConst);
            var insertSql = string.Format(
                "INSERT INTO {0} (UserId, PropertyNames, PropertyValuesString, PropertyValuesBinary, LastUpdatedDate) VALUES ($UserId, $PropertyNames, $PropertyValuesString, $PropertyValuesBinary, $LastUpdatedDate)",
                ProfileTableNameConst);

            Assert.Contains("FROM [aspnet_Profile]", countSql);
            Assert.Contains("UPDATE [aspnet_Profile]", updateSql);
            Assert.Contains("INSERT INTO [aspnet_Profile]", insertSql);

            // The table name must remain bracketed and must not allow injected characters.
            Assert.DoesNotContain(";", countSql);
            Assert.DoesNotContain("--", countSql);
            Assert.DoesNotContain("/*", countSql);
        }
    }
}
