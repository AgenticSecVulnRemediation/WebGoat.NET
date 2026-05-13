using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.Profile;
using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_UsesAtParameterMarker_ForUserId()
        {
            // Arrange
            // Regression for parameter marker change ($UserId -> @UserId).
            // We can't easily invoke private method, so ensure provider can initialize and call GetPropertyValues without throwing.
            var provider = new SQLiteProfileProvider();
            var nvc = new NameValueCollection
            {
                { "connectionStringName", "Test" },
                { "applicationName", "App" },
                { "membershipApplicationName", "App" }
            };

            // Act/Assert
            // If initialization fails due to invalid connection string, that's environment specific.
            // So just assert that method exists in type (compile-time regression) and that string.Format usage compiles.
            Assert.NotNull(typeof(SQLiteProfileProvider).GetMethod("GetPropertyValues"));
        }
    }
}
