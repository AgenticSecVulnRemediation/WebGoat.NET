using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderSetPropertyValuesTests
    {
        [Fact]
        public void SetPropertyValues_UsesNamedParametersForUserIdLookup_DoesNotThrow()
        {
            // Delta test: query changed from $Username/$ApplicationId to @Username/@ApplicationId.
            // We validate that executing SetPropertyValues no longer relies on $-prefixed parameter names.

            // Arrange
            var provider = new SQLiteProfileProvider();

            // Use shared in-memory db.
            var cs = "Data Source=file:profiledb?mode=memory&cache=shared";
            SetStaticField(typeof(SQLiteProfileProvider), "_connectionString", cs);
            SetStaticField(typeof(SQLiteProfileProvider), "_membershipApplicationId", Guid.NewGuid().ToString());

            using (var cn = new Mono.Data.Sqlite.SqliteConnection(cs))
            {
                cn.Open();
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE aspnet_Users (UserId TEXT, LoweredUsername TEXT, ApplicationId TEXT, LastActivityDate TEXT);";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "CREATE TABLE aspnet_Profile (UserId TEXT, PropertyNames TEXT, PropertyValuesString TEXT, PropertyValuesBinary BLOB, LastUpdatedDate TEXT);";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "INSERT INTO aspnet_Users (UserId, LoweredUsername, ApplicationId, LastActivityDate) VALUES ($uid,$un,$app,$dt);";
                    cmd.Parameters.AddWithValue("$uid", Guid.NewGuid().ToString());
                    cmd.Parameters.AddWithValue("$un", "bob");
                    cmd.Parameters.AddWithValue("$app", GetStaticField<string>(typeof(SQLiteProfileProvider), "_membershipApplicationId"));
                    cmd.Parameters.AddWithValue("$dt", DateTime.UtcNow.ToString("o"));
                    cmd.ExecuteNonQuery();
                }
            }

            // minimal settings context with one dirty property that can be saved
            var sc = new System.Configuration.SettingsContext();
            sc["UserName"] = "Bob";
            sc["IsAuthenticated"] = true;

            var prop = new System.Configuration.SettingsProperty("Theme");
            prop.PropertyType = typeof(string);
            prop.SerializeAs = System.Configuration.SettingsSerializeAs.String;
            prop.Attributes.Add("AllowAnonymous", true);

            var values = new System.Configuration.SettingsPropertyValueCollection();
            var spv = new System.Configuration.SettingsPropertyValue(prop)
            {
                SerializedValue = "dark",
                IsDirty = true
            };
            values.Add(spv);

            // Act
            var ex = Record.Exception(() => provider.SetPropertyValues(sc, values));

            // Assert
            Assert.Null(ex);
        }

        private static void SetStaticField(Type t, string fieldName, object value)
        {
            var f = t.GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic);
            Assert.NotNull(f);
            f!.SetValue(null, value);
        }

        private static T GetStaticField<T>(Type t, string fieldName)
        {
            var f = t.GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic);
            Assert.NotNull(f);
            return (T)f!.GetValue(null)!;
        }
    }
}
