using System;
using System.Reflection;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;
using Xunit;

// Delta-focused test: GetPropertyValuesFromDatabase switched from $param to @param names.
// Validate it still retrieves values using the new parameter names (and doesn't throw due to missing parameter bindings).

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProvider_GetPropertyValues_UsesAtParametersTests
    {
        private static void SetStaticField(Type t, string name, object? value)
        {
            var f = t.GetField(name, BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(f);
            f!.SetValue(null, value);
        }

        private static SQLiteProfileProvider CreateProvider(string cs)
        {
            var p = new SQLiteProfileProvider();
            SetStaticField(typeof(SQLiteProfileProvider), "_connectionString", cs);
            SetStaticField(typeof(SQLiteProfileProvider), "_membershipApplicationId", "app1");
            return p;
        }

        [Fact]
        public void GetPropertyValues_WithUserNameContainingQuote_DoesNotThrow()
        {
            var cs = "Data Source=file:memdb5?mode=memory&cache=shared";
            using var keeper = new SqliteConnection(cs);
            keeper.Open();

            using (var cmd = keeper.CreateCommand())
            {
                cmd.CommandText = @"
CREATE TABLE [aspnet_Users] (UserId TEXT, LoweredUsername TEXT, ApplicationId TEXT, UserName TEXT, IsAnonymous INTEGER, LastActivityDate TEXT);
CREATE TABLE [aspnet_Profile] (UserId TEXT, PropertyNames TEXT, PropertyValuesString TEXT, PropertyValuesBinary BLOB);
INSERT INTO [aspnet_Users](UserId, LoweredUsername, ApplicationId, UserName, IsAnonymous, LastActivityDate) VALUES ('u1', 'o''brien', 'app1', 'O''Brien', 0, '2020-01-01');
INSERT INTO [aspnet_Profile](UserId, PropertyNames, PropertyValuesString, PropertyValuesBinary) VALUES ('u1', 'P1:S:0:2:', 'v1', X'');
";
                cmd.ExecuteNonQuery();
            }

            var provider = CreateProvider(cs);

            var ctx = new SettingsContext();
            ctx["UserName"] = "O'Brien";
            ctx["IsAuthenticated"] = true;

            var props = new SettingsPropertyCollection();
            var sp = new SettingsProperty("P1")
            {
                PropertyType = typeof(string),
                SerializeAs = SettingsSerializeAs.String,
                Provider = provider,
                DefaultValue = ""
            };
            props.Add(sp);

            // Act
            var values = provider.GetPropertyValues(ctx, props);

            // Assert
            Assert.NotNull(values);
            Assert.Equal("v1", values["P1"]?.PropertyValue);
        }
    }
}
