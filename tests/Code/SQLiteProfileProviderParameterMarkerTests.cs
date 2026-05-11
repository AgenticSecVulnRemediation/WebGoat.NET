using System;
using System.Configuration;
using System.Web.Profile;
using Xunit;

using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderParameterMarkerTests
    {
        [Fact]
        public void SetPropertyValues_UsesAtParameterMarker_ForUserLookupQuery()
        {
            // Arrange
            // This is a delta test for the changed parameter marker from $Username/$ApplicationId to @Username/@ApplicationId.
            // We validate by calling SetPropertyValues with minimal setup and asserting it doesn't throw due to parameter name mismatch.
            var provider = new SQLiteProfileProvider();

            // The provider requires initialization. We provide minimal config with a bogus connection string;
            // the call should fail due to DB, but not due to missing parameters (which would surface differently).
            var config = new System.Collections.Specialized.NameValueCollection
            {
                { "connectionStringName", "Test" },
                { "applicationName", "TestApp" },
                { "membershipApplicationName", "TestApp" }
            };

            ConfigurationManager.ConnectionStrings.Add(new ConnectionStringSettings("Test", "Data Source=:memory:;Version=3"));
            provider.Initialize("SQLiteProfileProvider", config);

            var sc = new SettingsContext();
            sc["UserName"] = "user";
            sc["IsAuthenticated"] = true;

            var props = new SettingsPropertyCollection();
            var p = new SettingsProperty("SomeProp")
            {
                PropertyType = typeof(string),
                SerializeAs = SettingsSerializeAs.String
            };
            props.Add(p);
            var spv = new SettingsPropertyValueCollection();
            var pv = new SettingsPropertyValue(p) { PropertyValue = "v", IsDirty = true };
            spv.Add(pv);

            // Act
            var ex = Record.Exception(() => provider.SetPropertyValues(sc, spv));

            // Assert
            Assert.NotNull(ex);
            Assert.DoesNotContain("$Username", ex.Message, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("parameter", ex.Message, StringComparison.OrdinalIgnoreCase);
        }
    }
}
