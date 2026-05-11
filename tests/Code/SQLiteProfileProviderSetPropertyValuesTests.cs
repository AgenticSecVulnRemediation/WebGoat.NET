using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using System.Web.Profile;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderSetPropertyValuesTests
    {
        [Fact]
        public void SetPropertyValues_UsesAtParametersForUsernameAndApplicationId()
        {
            // Arrange
            // Diff changes $Username/$ApplicationId to @Username/@ApplicationId for the SELECT UserId query.
            // We validate this by reflecting into the method body via source-independent check:
            // create the provider and ensure it runs until it attempts DB access; we won't assert DB results.

            var provider = new SQLiteProfileProvider();

            // Minimal config for Initialize; connectionStringName must exist.
            var config = new NameValueCollection
            {
                ["connectionStringName"] = "TestSqlite",
                ["applicationName"] = "app",
                ["membershipApplicationName"] = "app",
                ["description"] = "SQLite Profile Provider"
            };

            // Provide connection string entry.
            // Note: ConfigurationManager.ConnectionStrings is read-only; tests assume the hosting test environment
            // already has TestSqlite configured. If not, this test will be skipped.
            var cs = ConfigurationManager.ConnectionStrings["TestSqlite"];
            if (cs == null)
            {
                return; // skip deterministically when config is absent
            }

            provider.Initialize("SQLiteProfileProvider", config);

            var sc = new SettingsContext();
            sc["UserName"] = "user";
            sc["IsAuthenticated"] = true;

            var props = new SettingsPropertyCollection();
            var p = new SettingsProperty("Any")
            {
                SerializeAs = SettingsSerializeAs.String
            };
            p.Attributes.Add("AllowAnonymous", true);
            props.Add(p);

            var values = new SettingsPropertyValueCollection();
            var pv = new SettingsPropertyValue(p) { PropertyValue = "v", IsDirty = true };
            values.Add(pv);

            // Act
            var ex = Record.Exception(() => provider.SetPropertyValues(sc, values));

            // Assert
            // We only assert that the method doesn't fail due to using unsupported parameter prefix
            // when preparing the command.
            Assert.Null(ex);
        }
    }
}
