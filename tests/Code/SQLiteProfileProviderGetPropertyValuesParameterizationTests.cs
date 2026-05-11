using System;
using System.Configuration;
using System.Web.Profile;
using Xunit;

using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderGetPropertyValuesParameterizationTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_UsesParameterizedUserIdLookup()
        {
            // Arrange
            // Delta test: changed query to string.Format with @UserId parameter.
            var provider = new SQLiteProfileProvider();
            var config = new System.Collections.Specialized.NameValueCollection
            {
                { "connectionStringName", "Test3" },
                { "applicationName", "TestApp" },
                { "membershipApplicationName", "TestApp" }
            };
            ConfigurationManager.ConnectionStrings.Add(new ConnectionStringSettings("Test3", "Data Source=:memory:;Version=3"));
            provider.Initialize("SQLiteProfileProvider", config);

            var sc = new SettingsContext();
            sc["UserName"] = "user";
            sc["IsAuthenticated"] = true;

            var properties = new SettingsPropertyCollection();
            properties.Add(new SettingsProperty("P") { PropertyType = typeof(string), SerializeAs = SettingsSerializeAs.String });

            // Act
            var ex = Record.Exception(() => provider.GetPropertyValues(sc, properties));

            // Assert
            Assert.NotNull(ex);
            Assert.DoesNotContain("$UserId", ex.Message, StringComparison.OrdinalIgnoreCase);
        }
    }
}
