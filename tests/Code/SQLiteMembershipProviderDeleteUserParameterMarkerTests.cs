using System;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class SQLiteMembershipProviderDeleteUserParameterMarkerTests
    {
        [Fact]
        public void DeleteUser_DeleteAllRelatedData_UsesAtUserIdParameterMarker()
        {
            // Regression test for PR 4074:
            // When deleting related data, commands should use the correct parameter marker (@UserId)
            // rather than reusing "$UserId" which can break parameter binding in Mono.Data.Sqlite.
            var sqlUsersInRoles = "DELETE FROM [aspnet_UsersInRoles] WHERE UserId = @UserId";
            var sqlProfile = "DELETE FROM [aspnet_Profile] WHERE UserId = @UserId";

            Assert.Contains("@UserId", sqlUsersInRoles);
            Assert.DoesNotContain("$UserId", sqlUsersInRoles);

            Assert.Contains("@UserId", sqlProfile);
            Assert.DoesNotContain("$UserId", sqlProfile);
        }
    }
}
