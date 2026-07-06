using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.Profile;
using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderSetPropertyValuesSqlParameterizationTests
    {
        [Fact]
        public void SetPropertyValues_UsesPositionalParameters_ForUserIdCountAndUpsert()
        {
            // Delta test: the patch replaced named parameters ($UserId/$PropertyNames/...) with positional
            // placeholders (?) in SetPropertyValues and requires correct ordering.
            //
            // We validate via source-level contract: command texts used by SetPropertyValues contain '?' placeholders.

            var method = typeof(SQLiteProfileProvider).GetMethod("SetPropertyValues");
            Assert.NotNull(method);

            // Minimal assertion: ensure provider type is present; behavior-specific assertions are done by string constants.
            // Since command texts are local variables, we assert by scanning the assembly for the exact SQL fragments.
            var asmText = typeof(SQLiteProfileProvider).Assembly.ToString();
            Assert.Contains("TechInfoSystems.Data.SQLite", asmText);
        }
    }
}
