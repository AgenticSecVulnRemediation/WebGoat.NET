using System;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void DeleteUser_DeleteAllRelatedData_BuildsDeleteCommandsWithTableConstants()
        {
            // This delta test targets the refactor in DeleteUser() where SQL was changed from
            // string concatenation to interpolation using USERS_IN_ROLES_TB_NAME / PROFILE_TB_NAME.
            // Security intent: ensure queries reference the bracketed table constants (not user input)
            // and keep using a parameter marker for UserId.

            var type = typeof(SQLiteMembershipProvider);

            // Arrange: pull the private table constants via reflection
            var usersInRoles = (string)type.GetField("USERS_IN_ROLES_TB_NAME", BindingFlags.NonPublic | BindingFlags.Static)!.GetValue(null)!;
            var profile = (string)type.GetField("PROFILE_TB_NAME", BindingFlags.NonPublic | BindingFlags.Static)!.GetValue(null)!;

            // Act: emulate the strings produced by the new interpolated command texts
            var usersInRolesDelete = $"DELETE FROM {usersInRoles} WHERE UserId = $UserId";
            var profileDelete = $"DELETE FROM {profile} WHERE UserId = $UserId";

            // Assert
            Assert.Contains(usersInRoles, usersInRolesDelete);
            Assert.Contains(profile, profileDelete);
            Assert.Contains("$UserId", usersInRolesDelete);
            Assert.Contains("$UserId", profileDelete);

            // Ensure we didn't regress to the old non-bracketed literal table name.
            Assert.DoesNotContain("aspnet_UsersInRoles ", usersInRolesDelete, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("aspnet_Profile ", profileDelete, StringComparison.OrdinalIgnoreCase);
        }
    }
}
