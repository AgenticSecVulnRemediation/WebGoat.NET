using System;
using System.IO;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderVerifyApplicationSqlRegressionTests
    {
        private static string FindRepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                if (Directory.Exists(Path.Combine(dir.FullName, "WebGoat")) && File.Exists(Path.Combine(dir.FullName, "WebGoat", "Code", "SQLiteProfileProvider.cs")))
                    return dir.FullName;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Could not locate repo root containing 'WebGoat/Code/SQLiteProfileProvider.cs'.");
        }

        [Fact]
        public void VerifyApplication_UsesHardCodedAspnetApplicationsTableAndAtParameterPrefix()
        {
            // Delta assertion: VerifyApplication now uses a fixed table name and @ parameters.
            // This removes reliance on APP_TB_NAME concatenation and aligns with parameterized command style.
            var root = FindRepoRoot();
            var file = Path.Combine(root, "WebGoat", "Code", "SQLiteProfileProvider.cs");
            var text = File.ReadAllText(file);

            Assert.Contains("INSERT INTO [aspnet_Applications] (ApplicationId, ApplicationName, Description) VALUES (@ApplicationId, @ApplicationName, @Description)", text);
            Assert.Contains("cmd.Parameters.AddWithValue (\"@ApplicationId\"", text);
            Assert.Contains("cmd.Parameters.AddWithValue (\"@ApplicationName\"", text);
            Assert.Contains("cmd.Parameters.AddWithValue (\"@Description\"", text);

            // Negative assertion: old $ApplicationId parameter usage in that command should not exist
            Assert.DoesNotContain("VALUES ($ApplicationId, $ApplicationName, $Description)", text);
        }
    }
}
