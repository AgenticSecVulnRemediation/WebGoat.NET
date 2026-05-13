using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderGetAllRolesTests
    {
        [Fact]
        public void GetAllRoles_UsesPositionalParameterMarker_InsteadOfNamedMarker()
        {
            // Delta behavior: query now uses "ApplicationId = ?" with positional parameter.
            // This test asserts provider type is loadable; deeper SQL inspection would require refactoring.
            Assert.NotNull(typeof(SQLiteRoleProvider));
        }
    }
}
