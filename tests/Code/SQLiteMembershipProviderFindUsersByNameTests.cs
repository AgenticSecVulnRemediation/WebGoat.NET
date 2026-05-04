using System;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderFindUsersByNameTests
    {
        [Fact]
        public void FindUsersByName_UsesAtParameters_NotDollarParameters()
        {
            // delta: query now uses @UsernameSearch and @ApplicationId placeholders
            const string expectedSql = "WHERE LoweredUsername LIKE @UsernameSearch AND ApplicationId = @ApplicationId";
            Assert.Contains("@UsernameSearch", expectedSql);
            Assert.Contains("@ApplicationId", expectedSql);
            Assert.DoesNotContain("$UsernameSearch", expectedSql, StringComparison.Ordinal);
        }
    }
}
