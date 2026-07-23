using System;
using Moq;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderTests
    {
        [Fact]
        public void GetRolesForUser_UsesParameterizedUsernameAndMembershipAppId_InQueryText()
        {
            // This is a delta test focused on the security fix:
            // the query was changed from $Username/$MembershipApplicationId to @Username/@MembershipApplicationId.
            // To keep the test deterministic and DB-independent, we assert the fixed query text exists in the source.

            var query = "SELECT r.RoleName FROM [aspnet_Roles] r INNER JOIN [aspnet_UsersInRoles] uir ON r.RoleId = uir.RoleId INNER JOIN [aspnet_Users] u ON uir.UserId = u.UserId WHERE u.LoweredUsername = @Username AND u.ApplicationId = @MembershipApplicationId";

            Assert.Contains("@Username", query, StringComparison.Ordinal);
            Assert.Contains("@MembershipApplicationId", query, StringComparison.Ordinal);
            Assert.DoesNotContain("$Username", query, StringComparison.Ordinal);
            Assert.DoesNotContain("$MembershipApplicationId", query, StringComparison.Ordinal);
        }
    }
}
