using System;
using System.Linq;
using System.Reflection;
using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderGetRolesForUserQueryTests
    {
        [Fact]
        public void GetRolesForUser_UsesFormattedJoinQueryAndParameterPlaceholders()
        {
            // Arrange
            var type = typeof(SQLiteRoleProvider);

            // Act
            // Delta behavior: query construction changed from concatenation to string.Format with table constants;
            // and parameters are bound for $Username and $MembershipApplicationId.
            var literals = type
                .GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
                .Select(f => f.GetRawConstantValue())
                .OfType<string>()
                .ToList();

            // Assert
            // Old concatenated fragments should not exist as a single literal anymore.
            Assert.DoesNotContain(literals, s => s.Contains("SELECT r.RoleName FROM \" + ROLE_TB_NAME", StringComparison.Ordinal));

            // New query should include the placeholders and JOINs.
            Assert.Contains(literals, s => s.Contains("SELECT r.RoleName FROM", StringComparison.Ordinal)
                                          && s.Contains("INNER JOIN", StringComparison.Ordinal)
                                          && s.Contains("$Username", StringComparison.Ordinal)
                                          && s.Contains("$MembershipApplicationId", StringComparison.Ordinal));
        }
    }
}
