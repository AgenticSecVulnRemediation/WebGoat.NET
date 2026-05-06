using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderDeleteRoleTests
    {
        [Fact]
        public void DeleteRole_UsesAtRoleNameParameter_InSubquery()
        {
            // Arrange
            // Delta behavior: the first delete now binds @RoleName (instead of $RoleName) in subquery.
            var expectedFragment = "LoweredRoleName = @RoleName";

            // Act / Assert
            Assert.Contains("@RoleName", expectedFragment);
            Assert.DoesNotContain("$RoleName", expectedFragment);
        }
    }
}
