using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderInterpolatedSqlTests
    {
        [Fact]
        public void SetPropertyValues_InterpolatedSql_StillUsesConstantTableName()
        {
            // Delta test: query construction changed from string concatenation to string interpolation
            // using PROFILE_TB_NAME constant.
            // Ensure PROFILE_TB_NAME remains the expected literal.

            var field = typeof(SQLiteProfileProvider).GetField("PROFILE_TB_NAME", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(field);
            Assert.Equal("[aspnet_Profile]", field.GetRawConstantValue());
        }
    }
}
