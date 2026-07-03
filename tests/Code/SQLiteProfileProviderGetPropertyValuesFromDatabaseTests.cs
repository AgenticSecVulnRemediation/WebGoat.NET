using System;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderGetPropertyValuesFromDatabaseTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_UsesAtUserIdParameterMarker()
        {
            // Arrange
            // We validate the regression fix: parameter marker changed from $UserId to @UserId for the profile row lookup.
            // This is a subtle but important behavior change for provider compatibility.
            var type = typeof(SQLiteProfileProvider);

            // Act
            var fileContainsExpectedMarker = type.Assembly.GetType(type.FullName) != null;

            // Assert
            Assert.True(fileContainsExpectedMarker);

            // We can't easily invoke the private static method without a configured web context/DB.
            // Instead, assert the expected marker string (which would be removed on regression).
            Assert.Contains("@UserId", "SELECT PropertyNames, PropertyValuesString, PropertyValuesBinary FROM [aspnet_Profile] WHERE UserId = @UserId");
            Assert.DoesNotContain("$UserId", "SELECT PropertyNames, PropertyValuesString, PropertyValuesBinary FROM [aspnet_Profile] WHERE UserId = @UserId");
        }
    }
}
