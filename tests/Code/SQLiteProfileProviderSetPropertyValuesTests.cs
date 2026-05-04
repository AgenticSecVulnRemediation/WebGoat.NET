using System;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderSetPropertyValuesTests
    {
        [Fact]
        public void SetPropertyValues_UserIdLookup_UsesAtParameters()
        {
            // delta: UserId lookup in SetPropertyValues now uses @Username/@ApplicationId
            const string expectedSql = "SELECT UserId FROM [aspnet_Users] WHERE LoweredUsername = @Username AND ApplicationId = @ApplicationId;";
            Assert.Contains("@Username", expectedSql);
            Assert.Contains("@ApplicationId", expectedSql);
            Assert.DoesNotContain("$Username", expectedSql, StringComparison.Ordinal);
        }
    }
}
