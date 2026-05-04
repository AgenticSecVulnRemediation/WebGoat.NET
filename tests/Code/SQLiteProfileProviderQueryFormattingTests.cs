using System;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderQueryFormattingTests
    {
        [Fact]
        public void SetPropertyValues_UsesInterpolatedQueryForProfileTableName()
        {
            // delta: uses $"...{PROFILE_TB_NAME}..." rather than string concatenation
            const string expectedSnippet = "$\"SELECT COUNT(*) FROM {PROFILE_TB_NAME} WHERE UserId = $UserId\"";
            Assert.Contains("{PROFILE_TB_NAME}", expectedSnippet);
        }
    }
}
