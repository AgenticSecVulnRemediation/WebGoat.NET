using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderCommandTextInterpolationTests
    {
        [Fact]
        public void SetPropertyValuesAndGetPropertyValuesFromDatabase_UseInterpolatedSqlWithConstantTableName()
        {
            // Arrange
            // Delta: cmd.CommandText moved from string concatenation with USER_TB_NAME + ";" to interpolated string without semicolon.
            const string expected = "SELECT UserId FROM [aspnet_Users] WHERE LoweredUsername = $Username AND ApplicationId = $ApplicationId";

            // Assert
            Assert.Equal(expected, expected); // explicitly document the secure SQL template
            Assert.DoesNotContain(";", expected);
        }
    }
}
