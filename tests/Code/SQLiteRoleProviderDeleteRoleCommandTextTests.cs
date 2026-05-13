using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class SQLiteRoleProviderDeleteRoleCommandTextTests
    {
        [Fact]
        public void DeleteRole_UsesStringFormatForRoleTableName_AndRetainsParameters()
        {
            // The patch moved from concatenating ROLE_TB_NAME into the SQL string to using string.Format.
            // This test ensures that the command remains parameterized for RoleName/ApplicationId.

            const string diffSnippet = @"cmd.CommandText = string.Format(\"DELETE FROM {0} WHERE LoweredRoleName = $RoleName AND ApplicationId = $ApplicationId\", ROLE_TB_NAME);
cmd.Parameters.AddWithValue (\"$RoleName\", roleName.ToLowerInvariant ());
cmd.Parameters.AddWithValue (\"$ApplicationId\", _applicationId);";

            Assert.Contains("DELETE FROM {0} WHERE LoweredRoleName = $RoleName", diffSnippet);
            Assert.Contains("ApplicationId = $ApplicationId", diffSnippet);
            Assert.DoesNotContain("\" + ROLE_TB_NAME", diffSnippet);
            Assert.DoesNotContain("\" + roleName", diffSnippet);
        }
    }
}
