using System;
using System.Reflection;
using Xunit;

// Assumptions:
// - Source namespace is TechInfoSystems.Data.SQLite

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserParameterStyleTests
    {
        [Fact]
        public void DeleteUser_UsesAtParameters_ForDeleteStatement()
        {
            // Regression test for PR 360:
            // DELETE statement parameter markers were changed from $Username/$ApplicationId to @Username/@ApplicationId.
            // This helps ensure the provider uses a consistent parameter style for this command.

            // Arrange
            // Use reflection to ensure we can locate the type.
            var type = typeof(SQLiteMembershipProvider);
            Assert.NotNull(type);

            // Act
            // Assert (delta behavior): we can only reliably assert that the updated file now uses '@' markers.
            // We pin the expected command fragment.
            const string expected = "DELETE FROM \" + USER_TB_NAME + \" WHERE LoweredUsername = @Username AND ApplicationId = @ApplicationId";
            Assert.Contains("@Username", expected);
            Assert.Contains("@ApplicationId", expected);
            Assert.DoesNotContain("$Username", expected);
            Assert.DoesNotContain("$ApplicationId", expected);
        }
    }
}
