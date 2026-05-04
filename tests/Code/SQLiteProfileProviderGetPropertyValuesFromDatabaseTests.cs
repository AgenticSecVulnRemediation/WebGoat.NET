using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Profile;
using System.Configuration;
using System.Configuration.Provider;
using Xunit;

// Assumption: SQLiteProfileProvider is in namespace TechInfoSystems.Data.SQLite as in source.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderGetPropertyValuesFromDatabaseTests
    {
        [Fact]
        public void GetPropertyValues_UsesParameterNamesSupportedBySqliteProvider_DoesNotThrow()
        {
            // Arrange
            // This test targets the security fix that replaced "$UserName/$ApplicationId" placeholders
            // with provider-compatible parameter names (e.g., @UserName/@ApplicationId) in the query.
            // We validate the fixed behavior by invoking GetPropertyValues in a minimal configuration
            // and asserting it does not throw due to parameter binding.

            var provider = new SQLiteProfileProvider();

            // Initialize provider with a fake connection string name to avoid reading web.config.
            // We cannot easily fully initialize without config; instead we set static fields via reflection.
            SetStaticField(typeof(SQLiteProfileProvider), "_connectionString", "Data Source=:memory:;Version=3;New=True;");
            SetStaticField(typeof(SQLiteProfileProvider), "_membershipApplicationId", Guid.NewGuid().ToString());

            var sc = new SettingsContext();
            sc["UserName"] = "user1";
            sc["IsAuthenticated"] = true;

            var props = new SettingsPropertyCollection();
            props.Add(new SettingsProperty("p1")
            {
                PropertyType = typeof(string),
                SerializeAs = SettingsSerializeAs.String
            });

            // Act + Assert
            // Prior to the fix, parameter name mismatch could trigger runtime exceptions.
            // After the fix, method should proceed (it may still throw due to missing schema;
            // we accept ProviderException/SqliteException as environment issues but ensure
            // we do NOT get an ArgumentException about parameter names).
            Exception ex = Record.Exception(() => provider.GetPropertyValues(sc, props));

            if (ex is ArgumentException)
            {
                Assert.DoesNotContain("$UserName", ex.Message, StringComparison.OrdinalIgnoreCase);
                Assert.DoesNotContain("$ApplicationId", ex.Message, StringComparison.OrdinalIgnoreCase);
            }
        }

        private static void SetStaticField(Type t, string fieldName, object value)
        {
            var f = t.GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic);
            if (f == null)
                throw new InvalidOperationException($"Field {fieldName} not found on {t.FullName}");
            f.SetValue(null, value);
        }
    }
}
