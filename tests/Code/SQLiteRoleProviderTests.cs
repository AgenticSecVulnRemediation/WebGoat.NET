using Xunit;

// Assumption: production code is in namespace TechInfoSystems.Data.SQLite as per source file.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderTests
    {
        [Fact]
        public void DeleteRole_UsesAtNamedParameters_ForRoleNameAndApplicationId()
        {
            // Arrange
            // Delta test for PR #4018: parameter markers changed from $RoleName/$ApplicationId to @RoleName/@ApplicationId.
            // Minimal regression check by scanning assembly text.

            var asm = typeof(SQLiteRoleProvider).Assembly;
            var bytes = System.IO.File.ReadAllBytes(asm.Location);
            var text = System.Text.Encoding.UTF8.GetString(bytes);

            // Act / Assert
            Assert.Contains("LoweredRoleName = @RoleName AND ApplicationId = @ApplicationId", text);
            Assert.DoesNotContain("LoweredRoleName = $RoleName AND ApplicationId = $ApplicationId", text);
        }
    }
}
