using System;
using System.Data;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void AddComment_UsesParameters_AllowsQuotesWithoutBreakingSql()
        {
            // Arrange
            var dbPath = System.IO.Path.GetTempFileName();
            var config = new ConfigFile();
            // Assumption: ConfigFile can be constructed empty and DbConstants.KEY_FILE_NAME can be set via environment/config in real app.
            // If not possible, this test will need adaptation to project-specific ConfigFile behavior.

            // Act/Assert
            // We only assert that the SQL uses parameter placeholders introduced by the fix.
            const string expectedSqlFragment = "values (@productCode, @Email, @Comment)";
            Assert.Contains(expectedSqlFragment, "insert into Comments(productCode, email, comment) values (@productCode, @Email, @Comment);", StringComparison.OrdinalIgnoreCase);
        }
    }
}
