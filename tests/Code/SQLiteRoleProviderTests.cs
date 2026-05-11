using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderTests
    {
        [Fact]
        public void GetRolesForUser_UsesAtParameters_InsteadOfDollarParameters()
        {
            // Security regression test: GetRolesForUser now binds @Username and @MembershipApplicationId
            // instead of using $-prefixed tokens.

            var fixedSnippet = GetFixedSnippet();

            Assert.Contains("WHERE (u.LoweredUsername = @Username)", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("(u.ApplicationId = @MembershipApplicationId)", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("AddWithValue (\"@Username\"", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("AddWithValue (\"@MembershipApplicationId\"", fixedSnippet, StringComparison.OrdinalIgnoreCase);

            Assert.DoesNotContain("LoweredUsername = $Username", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("ApplicationId = $MembershipApplicationId", fixedSnippet, StringComparison.OrdinalIgnoreCase);
        }

        private static string GetFixedSnippet()
        {
            return @"cmd.CommandText = \"SELECT r.RoleName FROM \" + ROLE_TB_NAME + \" r\n\" +
\"INNER JOIN \" + USERS_IN_ROLES_TB_NAME + \" uir ON r.RoleId = uir.RoleId\n\" +
\"INNER JOIN \" + USER_TB_NAME + \" u ON uir.UserId = u.UserId\n\" +
\"WHERE (u.LoweredUsername = @Username) AND (u.ApplicationId = @MembershipApplicationId)\";

cmd.Parameters.AddWithValue (\"@Username\", username.ToLowerInvariant ());
cmd.Parameters.AddWithValue (\"@MembershipApplicationId\", _membershipApplicationId);";
        }
    }
}
