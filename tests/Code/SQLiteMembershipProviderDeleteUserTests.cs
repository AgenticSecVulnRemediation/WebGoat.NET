using Xunit;

// Class under test
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserTests
    {
        [Fact]
        public void DeleteUser_QueryConstruction_UsesInterpolatedString_WithConstantTableName()
        {
            // Delta: query construction moved to interpolated string (no behavior change) but should keep table name constant.
            // We assert the constant is bracketed as expected, preventing accidental changes.
            var userTableNameField = typeof(SQLiteMembershipProvider).GetField("USER_TB_NAME", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(userTableNameField);
            var value = userTableNameField!.GetValue(null) as string;
            Assert.Equal("[aspnet_Users]", value);
        }
    }
}
