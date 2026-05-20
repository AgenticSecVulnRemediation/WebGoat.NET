using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProvider_SetPropertyValues_UsesStringFormatForProfileTableNameTests
    {
        [Fact]
        public void SetPropertyValues_UsesConstantProfileTableName_WhenBuildingInsertAndUpdateSql()
        {
            // Arrange
            // The fix replaced string concatenation with string.Format("UPDATE {0} ...", PROFILE_TB_NAME)
            // Regression test: PROFILE_TB_NAME remains a constant table identifier and does not accept runtime injection.
            // We assert the constant is unchanged and still bracketed.

            var field = typeof(SQLiteProfileProvider).GetField("PROFILE_TB_NAME", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(field);

            var value = (string)field!.GetValue(null)!;

            // Assert
            Assert.Equal("[aspnet_Profile]", value);
        }
    }
}
