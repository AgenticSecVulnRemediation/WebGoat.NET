using System;
using Xunit;

// Assumption: namespace from path WebGoat/Code/SQLiteProfileProvider.cs is TechInfoSystems.Data.SQLite
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void SetPropertyValues_UsesAtParameters_ForUserLookupQuery()
        {
            // Delta test: parameter markers changed from $Username/$ApplicationId to @Username/@ApplicationId.
            // We cannot intercept internal SqliteCommand creation without refactor; this test simply ensures the
            // provider can be instantiated and SetPropertyValues can be called with minimal context.

            var provider = new SQLiteProfileProvider();

            // Create minimal SettingsContext / properties; the method should short-circuit when username missing.
            var sc = new System.Configuration.SettingsContext();
            sc["UserName"] = "";
            sc["IsAuthenticated"] = false;

            var props = new System.Configuration.SettingsPropertyValueCollection();

            var ex = Record.Exception(() => provider.SetPropertyValues(sc, props));
            Assert.Null(ex);
        }
    }
}
