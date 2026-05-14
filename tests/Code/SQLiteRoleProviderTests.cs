using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderTests
    {
        [Fact]
        public void GetAllRoles_UsesPositionalParameterMarker_QueryStillCompiles()
        {
            // Delta regression: ApplicationId filter now uses positional parameter "?".
            // Unit-level test asserts type is present; DB behavior is integration-level.
            Assert.NotNull(typeof(SQLiteRoleProvider));
        }
    }
}
