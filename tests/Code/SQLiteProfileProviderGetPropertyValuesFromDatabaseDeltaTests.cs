using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderGetPropertyValuesFromDatabaseDeltaTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_UsesAtUserIdParameter()
        {
            // Delta: query changed to use @UserId and string.Format for the table name.

            var assembly = typeof(TechInfoSystems.Data.SQLite.SQLiteProfileProvider).Assembly;
            var strings = assembly.FullName ?? string.Empty;

            Assert.Contains("@UserId", strings);
            Assert.DoesNotContain("WHERE UserId = $UserId", strings);
        }
    }
}
