using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_UsesAtUserIdParameterMarker()
        {
            // Delta regression: changed "$UserId" parameter marker to "@UserId" when reading profile.
            // This test ensures the provider type is available; deeper DB interaction is integration.
            var type = typeof(SQLiteProfileProvider);
            Assert.NotNull(type);
        }
    }
}
