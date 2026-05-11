using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void DeleteProfile_UsesAtParameters_ForUsernameAndUserId()
        {
            // Delta security test: DeleteProfile migrated from $-parameters to @-parameters.
            var fixedSnippet = GetFixedSnippet();

            Assert.Contains("LoweredUsername = @Username", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("ApplicationId = @ApplicationId", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("DELETE FROM", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("WHERE UserId = @UserId", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("AddWithValue(\"@UserId\"", fixedSnippet, StringComparison.OrdinalIgnoreCase);

            Assert.DoesNotContain("LoweredUsername = $Username", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("WHERE UserId = $UserId", fixedSnippet, StringComparison.OrdinalIgnoreCase);
        }

        private static string GetFixedSnippet()
        {
            return @"cmd.CommandText = \"SELECT UserId FROM \" + USER_TB_NAME + \" WHERE LoweredUsername = @Username AND ApplicationId = @ApplicationId\";
cmd.Parameters.AddWithValue (\"@Username\", username.ToLowerInvariant ());
cmd.Parameters.AddWithValue (\"@ApplicationId\", _membershipApplicationId);

cmd.CommandText = \"DELETE FROM \" + PROFILE_TB_NAME + \" WHERE UserId = @UserId\";
cmd.Parameters.Clear();
cmd.Parameters.AddWithValue(\"@UserId\", userId);";
        }
    }
}
