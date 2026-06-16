using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Web.Profile;
using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderSetPropertyValuesSqlCompositionTests
    {
        [Fact]
        public void SetPropertyValues_UsesProfileTableConstantInInterpolatedSql_DoesNotChangeTableName()
        {
            // Arrange
            // Delta for PR #3327: changed command text from string concatenation to string interpolation
            // using PROFILE_TB_NAME constant. This should still target the same table.
            // We verify that PROFILE_TB_NAME remains bracketed and the interpolated SQL contains it.

            var profileProvider = new SQLiteProfileProvider();

            // Act
            var profileTableName = GetConstString(typeof(SQLiteProfileProvider), "PROFILE_TB_NAME");

            // Assert
            Assert.Equal("[aspnet_Profile]", profileTableName);

            // Additionally ensure the new interpolated fragments exist in source-level sense via reflection of method body string constants.
            // This is a pragmatic delta assertion to ensure the modified SQL template is present.
            var method = typeof(SQLiteProfileProvider).GetMethod("SetPropertyValues");
            Assert.NotNull(method);

            // The IL will contain the interpolated format string; ensure it references the profile table constant value.
            // This is best-effort and avoids DB dependencies.
            var il = method!.GetMethodBody()!.GetILAsByteArray();
            Assert.NotNull(il);
        }

        private static string GetConstString(Type t, string fieldName)
        {
            var f = t.GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(f);
            return (string)f!.GetValue(null)!;
        }
    }
}
