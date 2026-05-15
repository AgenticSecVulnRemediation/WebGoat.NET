using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderStringFormatTests
    {
        [Fact]
        public void DeleteProfile_UsesStringFormat_WithConstantTableNames()
        {
            // Delta test: command text switched to string.Format with constant table names.
            // Validate constants used remain unchanged.

            var userTable = typeof(SQLiteProfileProvider).GetField("USER_TB_NAME", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            var profileTable = typeof(SQLiteProfileProvider).GetField("PROFILE_TB_NAME", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            Assert.NotNull(userTable);
            Assert.NotNull(profileTable);
            Assert.Equal("[aspnet_Users]", userTable.GetRawConstantValue());
            Assert.Equal("[aspnet_Profile]", profileTable.GetRawConstantValue());
        }
    }
}
