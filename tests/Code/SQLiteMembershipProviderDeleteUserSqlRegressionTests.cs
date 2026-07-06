using System;
using System.IO;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserSqlRegressionTests
    {
        private static string FindRepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                if (Directory.Exists(Path.Combine(dir.FullName, "WebGoat")) && File.Exists(Path.Combine(dir.FullName, "WebGoat", "Code", "SQLiteMembershipProvider.cs")))
                    return dir.FullName;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Could not locate repo root containing 'WebGoat/Code/SQLiteMembershipProvider.cs'.");
        }

        [Fact]
        public void DeleteUser_WhenSelectingUserIdForCascadeDelete_ClearsParametersBeforeNextCommand()
        {
            // Delta assertion: fixed code must clear parameters after executing the SELECT with @Username/@ApplicationId.
            // This prevents parameter pollution when the same SqliteCommand is reused for subsequent DELETE statements.
            var root = FindRepoRoot();
            var file = Path.Combine(root, "WebGoat", "Code", "SQLiteMembershipProvider.cs");
            var text = File.ReadAllText(file);

            Assert.Contains("SELECT UserId FROM \" + USER_TB_NAME + \" WHERE LoweredUsername = @Username AND ApplicationId = @ApplicationId", text);
            Assert.Contains("cmd.Parameters.AddWithValue (\"@Username\"", text);
            Assert.Contains("cmd.Parameters.AddWithValue (\"@ApplicationId\"", text);

            // Key fix line
            Assert.Contains("cmd.Parameters.Clear()", text);
        }
    }
}
