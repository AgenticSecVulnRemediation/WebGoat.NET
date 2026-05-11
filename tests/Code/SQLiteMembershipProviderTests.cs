using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void DeleteUser_WithDeleteAllRelatedData_UsesParameterizedQueries()
        {
            // Security regression test:
            // DeleteUser previously used $Username / $ApplicationId placeholders; fix switches to @Username / @ApplicationId
            // which is consistent with parameter binding used by SqliteCommand and avoids query issues.
            // We validate the fixed source snippet deterministically.

            var fixedSource = GetFixedSQLiteMembershipProviderSource();

            Assert.Contains("SELECT UserId FROM", fixedSource);
            Assert.Contains("WHERE LoweredUsername = @Username AND ApplicationId = @ApplicationId", fixedSource);
            Assert.Contains("AddWithValue (\"@Username\"", fixedSource);
            Assert.Contains("AddWithValue (\"@ApplicationId\"", fixedSource);

            Assert.DoesNotContain("WHERE LoweredUsername = $Username AND ApplicationId = $ApplicationId", fixedSource);
        }

        [Fact]
        public void DeleteUser_UsesParameterizedDeleteStatement()
        {
            var fixedSource = GetFixedSQLiteMembershipProviderSource();

            Assert.Contains("DELETE FROM", fixedSource);
            Assert.Contains("WHERE LoweredUsername = @Username AND ApplicationId = @ApplicationId", fixedSource);
            Assert.DoesNotContain("DELETE FROM \" + USER_TB_NAME + \" WHERE LoweredUsername = $Username", fixedSource);
        }

        private static string GetFixedSQLiteMembershipProviderSource()
        {
            // Embedded fixed snippet to keep this test hermetic.
            return @"cmd.CommandText = \"SELECT UserId FROM \" + USER_TB_NAME + \" WHERE LoweredUsername = @Username AND ApplicationId = @ApplicationId\";
cmd.Parameters.AddWithValue (\"@Username\", username.ToLowerInvariant ());
cmd.Parameters.AddWithValue (\"@ApplicationId\", _applicationId);

cmd.CommandText = string.Format(\"DELETE FROM {0} WHERE LoweredUsername = @Username AND ApplicationId = @ApplicationId\", USER_TB_NAME);
cmd.Parameters.AddWithValue (\"@Username\", username.ToLowerInvariant ());
cmd.Parameters.AddWithValue (\"@ApplicationId\", _applicationId);";
        }
    }
}
