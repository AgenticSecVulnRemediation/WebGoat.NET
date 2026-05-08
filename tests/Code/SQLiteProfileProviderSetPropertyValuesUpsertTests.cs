using System;
using System.Text;
using Xunit;

using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderSetPropertyValuesUpsertTests
    {
        [Fact]
        public void SetPropertyValues_UsesPositionalPlaceholdersForUpdateAndInsert()
        {
            // Delta regression test: UPDATE/INSERT SQL changed to use positional placeholders (?), and parameters are cleared/added.
            var asmBytes = System.IO.File.ReadAllBytes(typeof(SQLiteProfileProvider).Assembly.Location);
            var asmText = Encoding.UTF8.GetString(asmBytes);

            Assert.Contains("SET PropertyNames = ?", asmText);
            Assert.Contains("VALUES (?, ?, ?, ?, ?)", asmText);
        }
    }
}
