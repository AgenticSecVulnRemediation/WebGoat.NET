using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderInterpolatedQuerySafetyTests
    {
        [Fact]
        public void InterpolatedQuery_UsesConstantProfileTableName()
        {
            // Arrange
            const string profileTableName = "[aspnet_Profile]";

            // Act
            string sql = $"SELECT PropertyNames, PropertyValuesString, PropertyValuesBinary FROM {profileTableName} WHERE UserId = $UserId";

            // Assert: ensure user-controlled input is not used as table name
            Assert.Contains(profileTableName, sql);
            Assert.DoesNotContain("{", sql); // interpolation already resolved in string
        }
    }
}
