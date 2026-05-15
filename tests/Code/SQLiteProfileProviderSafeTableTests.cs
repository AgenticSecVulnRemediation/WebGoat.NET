using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderSafeTableTests
    {
        [Fact]
        public void SetPropertyValues_UsesSafeProfileTableConstant_ForProfileCommands()
        {
            // Delta test: new safeProfileTable constant is used instead of PROFILE_TB_NAME for UPDATE/INSERT/COUNT.
            // This is a structural change; we assert that the constant exists via reflection.

            var field = typeof(SQLiteProfileProvider).GetField("safeProfileTable", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(field);
            Assert.Equal("[aspnet_Profile]", field.GetRawConstantValue());
        }
    }
}
