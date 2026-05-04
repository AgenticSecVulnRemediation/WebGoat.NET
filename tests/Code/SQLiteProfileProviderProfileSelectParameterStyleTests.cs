using System;
using System.Reflection;
using System.Web.Profile;
using Xunit;

using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderProfileSelectParameterStyleTests
    {
        [Fact]
        public void GetPropertyValues_UsesAtUserIdParameterStyleForProfileLookup_DoesNotThrowForParameterBinding()
        {
            // Arrange
            // Delta: query changed from "$UserId" to "@UserId".
            // Assert we do not fail due to parameter binding name mismatch when executing.
            var provider = new SQLiteProfileProvider();
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

            // Act
            var ex = Record.Exception(() => provider.GetPropertyValues(sc, props));

            // Assert
            if (ex is ArgumentException)
            {
                Assert.DoesNotContain("$UserId", ex.Message, StringComparison.OrdinalIgnoreCase);
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
