using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserSqlRegressionTests
    {
        [Fact]
        public void DeleteUser_UsesStringFormatForDeleteStatements()
        {
            // Arrange
            var fileText = EmbeddedSourceReader.Read("WebGoat/Code/SQLiteMembershipProvider.cs");

            // Assert: delta in PR uses string.Format(...) for DELETE statements.
            Assert.Contains("cmd.CommandText = string.Format(\"DELETE FROM {0} WHERE LoweredUsername = $Username AND ApplicationId = $ApplicationId\"", fileText);
            Assert.Contains("cmd.CommandText = string.Format(\"DELETE FROM {0} WHERE UserId = $UserId\"", fileText);

            // Previously it was plain concatenation for some DELETE statements.
            Assert.DoesNotContain("cmd.CommandText = \"DELETE FROM \" + USER_TB_NAME + \" WHERE LoweredUsername", fileText);
            Assert.DoesNotContain("cmd.CommandText = \"DELETE FROM \" + USERS_IN_ROLES_TB_NAME + \" WHERE UserId", fileText);
        }
    }

    internal static class EmbeddedSourceReader
    {
        public static string Read(string relativePath)
        {
            var candidates = new[] { AppContext.BaseDirectory, System.IO.Directory.GetCurrentDirectory() };
            foreach (var baseDir in candidates)
            {
                var path = System.IO.Path.GetFullPath(System.IO.Path.Combine(baseDir, "..", "..", "..", "..", relativePath));
                if (System.IO.File.Exists(path))
                    return System.IO.File.ReadAllText(path);

                path = System.IO.Path.GetFullPath(System.IO.Path.Combine(baseDir, "..", "..", "..", relativePath));
                if (System.IO.File.Exists(path))
                    return System.IO.File.ReadAllText(path);
            }
            throw new InvalidOperationException($"Could not locate source file: {relativePath}");
        }
    }
}
