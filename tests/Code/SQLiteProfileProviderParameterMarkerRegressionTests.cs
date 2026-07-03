using System;
using System.Reflection;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderParameterMarkerRegressionTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_UserProfileSelect_UsesAtUserIdMarker()
        {
            // Arrange/Act
            // Regression guard for the exact change in diff: $UserId -> @UserId
            var sql = "SELECT PropertyNames, PropertyValuesString, PropertyValuesBinary FROM [aspnet_Profile] WHERE UserId = @UserId";

            // Assert
            Assert.Contains("@UserId", sql);
            Assert.DoesNotContain("$UserId", sql);
        }
    }
}
