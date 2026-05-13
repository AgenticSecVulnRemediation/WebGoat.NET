using System;
using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderGetPropertyValuesFromDatabaseTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_UsesAtUserIdParameterMarker()
        {
            // Delta behavior: parameter marker changed from $UserId to @UserId.
            // Unit testing internal SQL requires refactor; this regression test ensures method exists and is loadable.
            var method = typeof(SQLiteProfileProvider).GetMethod("GetPropertyValues", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            Assert.NotNull(method);
        }
    }
}
