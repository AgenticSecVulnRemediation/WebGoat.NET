using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderDeleteProfileTests
    {
        [Fact]
        public void DeleteProfile_UsesAtUserIdParameter_NotDollarUserId()
        {
            // Delta test for PR #3981: DeleteProfile uses "@UserId" parameter placeholder
            // rather than "$UserId". This prevents parameter mismatches and accidental
            // string concatenation regressions.

            var sourcePath = System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "WebGoat", "Code", "SQLiteProfileProvider.cs");
            var text = System.IO.File.ReadAllText(sourcePath);

            Assert.Contains("WHERE UserId = @UserId", text);
            Assert.Contains("cmd.Parameters.Add (\"@UserId\"", text);

            // Ensure the old placeholder isn't used in the delete statement.
            Assert.DoesNotContain("WHERE UserId = $UserId\"", text);
            Assert.DoesNotContain("cmd.Parameters.Add (\"$UserId\"", text);
        }
    }
}
