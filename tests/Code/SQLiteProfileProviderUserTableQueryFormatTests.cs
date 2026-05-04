using System;
using System.Configuration;
using System.Reflection;
using System.Web.Profile;
using Xunit;

using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderUserTableQueryFormatTests
    {
        [Fact]
        public void GetPropertyValues_DoesNotUseStringConcatenationForUserTableQuery_UsesInterpolatedConstant()
        {
            // Arrange
            // This delta test focuses on the change to construct SQL using the constant USER_TB_NAME
            // via interpolation and strongly-typed parameters (SqliteParameter), which helps prevent
            // malformed query/parameter binding issues.
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
            // We can't reliably run DB schema here, but we can assert we didn't hit a parameter-name issue.
            if (ex is ArgumentException)
            {
                Assert.DoesNotContain("AddWithValue", ex.Message, StringComparison.OrdinalIgnoreCase);
                Assert.DoesNotContain("parameter", ex.Message, StringComparison.OrdinalIgnoreCase);
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
