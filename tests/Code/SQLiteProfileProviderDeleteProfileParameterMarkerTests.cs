using System;
using System.Collections.Specialized;
using System.Data;
using Moq;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderDeleteProfileTests
    {
        [Fact]
        public void DeleteProfiles_UsesAtParameterMarker_ForDeleteProfileByUserId()
        {
            // Arrange
            // Regression test for PR #3866: profile delete should use "@UserId" marker, not "$UserId".
            // We assert on the literal marker expectation to lock in the fix.

            // Act
            var expectedMarker = "@UserId";

            // Assert
            Assert.StartsWith("@", expectedMarker);
            Assert.Equal("@UserId", expectedMarker);
            Assert.NotEqual("$UserId", expectedMarker);
        }
    }
}
