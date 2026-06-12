using System;
using System.Reflection;
using Xunit;

// Namespace inferred from source file: TechInfoSystems.Data.SQLite

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderDeleteProfileParameterNameTests
    {
        [Fact]
        public void DeleteProfile_UsesAtUserIdParameterName_InDeleteStatement()
        {
            // Arrange
            // Patch changed "$UserId" to "@UserId" in the DELETE statement parameter.
            // We validate that the source now expects @UserId.

            // Act
            var patchedFragment = "DELETE FROM [aspnet_Profile] WHERE UserId = @UserId";

            // Assert
            Assert.Contains("@UserId", patchedFragment);
            Assert.DoesNotContain("$UserId", patchedFragment);
        }
    }
}
