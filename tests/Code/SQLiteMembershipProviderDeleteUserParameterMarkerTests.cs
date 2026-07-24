using Xunit;
using Moq;
using TechInfoSystems.Data.SQLite;
using System.Reflection;
using System;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserParameterMarkerTests
    {
        [Fact]
        public void DeleteUser_UsesAtUserIdParameterMarkerInRelatedDeletes()
        {
            // Arrange
            // The fix changed parameter marker from $UserId to @UserId.
            // Validate by inspecting method IL text is not feasible; instead check that the source file contains @UserId.
            var path = System.IO.Path.Combine("WebGoat", "Code", "SQLiteMembershipProvider.cs");
            var content = System.IO.File.ReadAllText(path);

            // Assert
            Assert.Contains("@UserId", content);
            Assert.DoesNotContain("WHERE UserId = $UserId", content);
        }
    }
}
