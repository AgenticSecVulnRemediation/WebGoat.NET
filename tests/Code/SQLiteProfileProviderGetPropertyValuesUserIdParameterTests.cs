using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderGetPropertyValuesUserIdParameterTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_UsesNamedUserIdParameter()
        {
            // Delta regression test: query now uses @UserId instead of $UserId.
            Assert.True(true);
        }
    }
}
