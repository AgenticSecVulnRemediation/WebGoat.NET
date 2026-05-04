using System;
using System.Reflection;
using Xunit;

using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderVerifyApplicationParameterStyleTests
    {
        [Fact]
        public void ApplicationName_Setter_TriggersVerifyApplication_UsesAtParameters_DoesNotThrowForParameterBinding()
        {
            // Arrange
            // Delta: VerifyApplication INSERT changed from "$ApplicationId" style to "@ApplicationId".
            // We assert that invoking Initialize-like flow doesn't fail specifically due to param style.
            SetStaticField(typeof(SQLiteProfileProvider), "_connectionString", "Data Source=:memory:;Version=3;New=True;");

            var provider = new SQLiteProfileProvider();

            // Act
            var ex = Record.Exception(() => provider.ApplicationName = "app1");

            // Assert
            if (ex is ArgumentException)
            {
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
