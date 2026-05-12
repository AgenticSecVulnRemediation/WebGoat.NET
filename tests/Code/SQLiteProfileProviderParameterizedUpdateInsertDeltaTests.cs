using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderParameterizedUpdateInsertDeltaTests
    {
        [Fact]
        public void SQLiteProfileProvider_SetPropertyValues_UsesPositionalPlaceholders_ForUpdateAndInsert()
        {
            // Delta: UPDATE/INSERT for aspnet_Profile changed to positional placeholders.
            // Assert the new SQL fragments are present in the provider assembly strings.

            var assembly = typeof(TechInfoSystems.Data.SQLite.SQLiteProfileProvider).Assembly;
            var strings = assembly.FullName ?? string.Empty;

            Assert.Contains("SET PropertyNames = ?, PropertyValuesString = ?, PropertyValuesBinary = ?, LastUpdatedDate = ? WHERE UserId = ?", strings);
            Assert.Contains("VALUES (?, ?, ?, ?, ?)", strings);
        }
    }
}
