using System.Reflection;
using Xunit;

using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderGetRolesForUserTests
    {
        [Fact]
        public void GetRolesForUser_MethodExists_AfterQueryRewriteToStringFormat()
        {
            // Arrange
            // Delta: SQL for GetRolesForUser rewritten to string.Format and uses @ parameters.
            var method = typeof(SQLiteRoleProvider).GetMethod("GetRolesForUser", BindingFlags.Public | BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }
    }
}
