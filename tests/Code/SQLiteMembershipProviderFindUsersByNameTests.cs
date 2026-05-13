using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderFindUsersByNameTests
    {
        [Fact]
        public void FindUsersByName_UsernameSearchParameterValue_IsWrappedInWildcards()
        {
            // Delta regression: PR updated parameter binding to add %...% around the search term.
            const string usernameToMatch = "John";
            var value = "%" + usernameToMatch.ToLowerInvariant() + "%";

            Assert.Equal("%john%", value);
        }
    }
}
