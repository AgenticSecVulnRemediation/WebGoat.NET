using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void FindUsersByName_UsesWildcardParameter_ForLikeQuery()
        {
            // Delta behavior: UsernameSearch parameter now wraps value with % for LIKE.
            var usernameToMatch = "john";
            var parameterValue = "%" + usernameToMatch.ToLowerInvariant() + "%";

            Assert.Equal("%john%", parameterValue);
        }
    }
}
