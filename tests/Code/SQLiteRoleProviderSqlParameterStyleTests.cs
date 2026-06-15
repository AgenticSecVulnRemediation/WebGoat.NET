using Xunit;
using TechInfoSystems.Data.SQLite;
using System.Text.RegularExpressions;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderSqlParameterStyleTests
    {
        [Fact]
        public void SQLiteRoleProvider_UsesAtParametersInVerifyApplicationInsert()
        {
            // Regression test for parameter marker change in VerifyApplication(): $param -> @param.
            // We validate by checking for the @ApplicationId marker in the source file.
            // NOTE: This is a code-level regression test that reads the file content.

            var path = System.IO.Path.Combine("WebGoat", "Code", "SQLiteRoleProvider.cs");
            var content = System.IO.File.ReadAllText(path);

            Assert.Contains("VALUES (@ApplicationId, @ApplicationName, @Description)", content);
            Assert.DoesNotContain("VALUES ($ApplicationId", content);
        }
    }
}
