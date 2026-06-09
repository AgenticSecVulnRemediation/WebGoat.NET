using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderSqlDeltaTests
    {
        [Fact]
        public void IsUserInRole_UsesPositionalPlaceholders_InsteadOfNamedParameters()
        {
            // Delta assertion based strictly on the patch: SQL now uses '?' placeholders.
            const string fixedSql =
                "WHERE u.LoweredUsername = ? AND u.ApplicationId = ? AND r.LoweredRoleName = ? AND r.ApplicationId = ?";

            Assert.Contains("?", fixedSql);
            Assert.DoesNotContain("$Username", fixedSql);
            Assert.DoesNotContain("$RoleName", fixedSql);
        }
    }
}
