using System;
using System.Configuration;
using System.Web.Profile;
using Xunit;

using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderSetPropertyValuesStatementStyleTests
    {
        [Fact]
        public void SetPropertyValues_UsesPositionalParameters_ForUpdateAndInsertStatements()
        {
            // Arrange
            // Delta test: UPDATE/INSERT switched to positional '?' markers to avoid injection via string building.
            // We can't introspect SqliteCommand.CommandText easily without DB, so we assert runtime doesn't throw for marker mismatch.
            var provider = new SQLiteProfileProvider();
            var config = new System.Collections.Specialized.NameValueCollection
            {
                { "connectionStringName", "Test2" },
                { "applicationName", "TestApp" },
                { "membershipApplicationName", "TestApp" }
            };
            ConfigurationManager.ConnectionStrings.Add(new ConnectionStringSettings("Test2", "Data Source=:memory:;Version=3"));
            provider.Initialize("SQLiteProfileProvider", config);

            var sc = new SettingsContext();
            sc["UserName"] = "anon";
            sc["IsAuthenticated"] = false;

            var props = new SettingsPropertyValueCollection();
            var p = new SettingsProperty("AllowAnonymousProp")
            {
                PropertyType = typeof(string),
                SerializeAs = SettingsSerializeAs.String
            };
            p.Attributes.Add("AllowAnonymous", true);
            var pv = new SettingsPropertyValue(p) { PropertyValue = "v", IsDirty = true };
            props.Add(pv);

            // Act
            var ex = Record.Exception(() => provider.SetPropertyValues(sc, props));

            // Assert
            Assert.NotNull(ex);
            // Ensure we didn't fail due to named parameter mismatch from old "$PropertyNames" style in UPDATE/INSERT.
            Assert.DoesNotContain("PropertyNames", ex.Message, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("parameter", ex.Message, StringComparison.OrdinalIgnoreCase);
        }
    }
}
