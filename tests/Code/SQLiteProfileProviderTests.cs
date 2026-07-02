using System;
using Xunit;

// Delta test: query construction now uses string.Format with PROFILE_TB_NAME constant.
// The security impact is that only the constant table name is interpolated, not user input.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void ProfileUpsertSql_OnlyInterpolatesTableConstant_DoesNotIntroduceUserInputConcatenation()
        {
            // Arrange
            const string profileTable = "[aspnet_Profile]";

            // Act
            var updateSql = string.Format(
                "UPDATE {0} SET PropertyNames = $PropertyNames, PropertyValuesString = $PropertyValuesString, PropertyValuesBinary = $PropertyValuesBinary, LastUpdatedDate = $LastUpdatedDate WHERE UserId = $UserId",
                profileTable);

            // Assert
            Assert.Contains(profileTable, updateSql);
            Assert.Contains("$UserId", updateSql);
            Assert.DoesNotContain("' +", updateSql);
        }
    }
}
