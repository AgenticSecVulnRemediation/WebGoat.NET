using System;
using Xunit;

// Assumption: SQLiteProfileProvider is in namespace TechInfoSystems.Data.SQLite.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderParameterMarkerTests
    {
        [Fact]
        public void GetPropertyValues_UsernameWithQuote_DoesNotBreakSqlParameterMarkers()
        {
            // Arrange
            var provider = new SQLiteProfileProvider();

            // Act/Assert
            // We validate the change to @UserName/@ApplicationId parameter markers by ensuring the method
            // can be invoked with an input containing a quote without throwing due to malformed SQL.
            var context = new System.Configuration.SettingsContext();
            context["UserName"] = "a'user";
            context["IsAuthenticated"] = false;

            var properties = new System.Configuration.SettingsPropertyCollection();
            var ex = Record.Exception(() => provider.GetPropertyValues(context, properties));

            Assert.Null(ex);
        }
    }
}
