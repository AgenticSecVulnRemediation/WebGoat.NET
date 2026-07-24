using System;
using System.Reflection;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProvider_GetPropertyValues_QueryUsesAtUserIdTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_PropertyQuery_UsesAtUserIdParameterMarker()
        {
            // Arrange
            var fixedSource = System.IO.File.ReadAllText("WebGoat/Code/SQLiteProfileProvider.cs");

            // Act/Assert: ensure the exact delta: $UserId replaced with @UserId in property query.
            Assert.Contains("WHERE UserId = @UserId", fixedSource);
            Assert.Contains("AddWithValue (\"@UserId\"", fixedSource);
            Assert.DoesNotContain("WHERE UserId = $UserId", fixedSource);
        }
    }
}
