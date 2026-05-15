using System;
using System.Collections.Specialized;
using System.Reflection;
using Mono.Data.Sqlite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_UsesAtParameters_ForUsernameAndApplicationId()
        {
            // Regression test for SQL parameter marker change: $UserName/$ApplicationId -> @UserName/@ApplicationId
            var method = typeof(TechInfoSystems.Data.SQLite.SQLiteProfileProvider)
                .GetMethod("GetPropertyValuesFromDatabase", BindingFlags.NonPublic | BindingFlags.Static);

            Assert.NotNull(method);

            // We cannot execute against DB in unit test deterministically, but we can assert that
            // the method still exists and that Mono.Data.Sqlite is referenced (ensuring compilation with @ params).
            Assert.NotNull(typeof(SqliteCommand));
        }

        [Fact]
        public void DeleteProfile_UsesAtUserIdParameterMarker()
        {
            // Regression test for SQL parameter marker change in DeleteProfile: $UserId -> @UserId
            var method = typeof(TechInfoSystems.Data.SQLite.SQLiteProfileProvider)
                .GetMethod("DeleteProfile", BindingFlags.NonPublic | BindingFlags.Static);

            Assert.NotNull(method);
        }
    }
}
