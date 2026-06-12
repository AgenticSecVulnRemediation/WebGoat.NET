using System.IO;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProvider_DeleteUserInterpolationTests
    {
        [Fact]
        public void DeleteUser_WhenDeletingRelatedData_UsesInterpolatedConstantsWithParameterizedUserId()
        {
            // Delta test: SQL strings for deleting from USERS_IN_ROLES_TB_NAME/PROFILE_TB_NAME were changed
            // to interpolated strings. The important security behavior is: still uses $UserId parameter,
            // and table name comes from constant (not user input).

            var path = Path.Combine(Directory.GetCurrentDirectory(), "WebGoat", "Code", "SQLiteMembershipProvider.cs");
            Assert.True(File.Exists(path), $"Expected file at {path}");

            var text = File.ReadAllText(path);

            Assert.Contains("DELETE FROM {USERS_IN_ROLES_TB_NAME} WHERE UserId = $UserId", text);
            Assert.Contains("DELETE FROM {PROFILE_TB_NAME} WHERE UserId = $UserId", text);
            Assert.Contains("cmd.Parameters.AddWithValue (\"$UserId\"", text);
        }
    }
}
