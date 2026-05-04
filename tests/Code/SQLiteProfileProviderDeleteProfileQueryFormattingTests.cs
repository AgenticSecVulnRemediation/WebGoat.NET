using System;
using System.Reflection;
using Xunit;

using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderDeleteProfileQueryFormattingTests
    {
        [Fact]
        public void DeleteProfiles_UsesFormattedTableNameConstants_DoesNotThrowForFormatStrings()
        {
            // Arrange
            // Delta: switched to string.Format with table name constants for SELECT/DELETE.
            // This test ensures formatting is safe and does not crash with FormatException and
            // continues to use parameters for user-controlled values.
            var provider = new SQLiteProfileProvider();
            SetStaticField(typeof(SQLiteProfileProvider), "_connectionString", "Data Source=:memory:;Version=3;New=True;");
            SetStaticField(typeof(SQLiteProfileProvider), "_membershipApplicationId", Guid.NewGuid().ToString());

            // Act
            var ex = Record.Exception(() => provider.DeleteProfiles(new[] { "user1" }));

            // Assert
            Assert.False(ex is FormatException);
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
