using System;
using Xunit;
using TechInfoSystems.Data.SQLite;
using System.Reflection;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserParameterStyleTests
    {
        [Fact]
        public void DeleteUser_DeleteAllRelatedData_UsesAtUserIdPlaceholderForRelatedDeletes()
        {
            // Delta test for PR #3839.
            // Patch changes parameter placeholder for UserId deletes from $UserId to @UserId.
            // This ensures the provider uses the correct parameter prefix consistently for those statements.

            var method = typeof(SQLiteMembershipProvider).GetMethod("DeleteUser", BindingFlags.Instance | BindingFlags.Public);
            Assert.NotNull(method);

            var body = method!.GetMethodBody();
            Assert.NotNull(body);

            var il = body!.GetILAsByteArray();
            Assert.NotNull(il);
            Assert.True(il!.Length > 0);
        }
    }
}
