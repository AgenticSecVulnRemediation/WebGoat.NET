using System;
using Mono.Data.Sqlite;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderAddCommentParameterizedTests
    {
        [Fact]
        public void AddComment_WithInjectionPayload_DoesNotThrowSqlSyntaxErrorFromConcatenation()
        {
            // Delta intent: INSERT now parameterized.
            var cfg = new ConfigFile();
            cfg.Set(DbConstants.KEY_FILE_NAME, "test.db");
            cfg.Set(DbConstants.KEY_CLIENT_EXEC, "sqlite3");

            var provider = new SqliteDbProvider(cfg);

            var ex = Record.Exception(() => provider.AddComment("p", "a@b.com", "x'); DROP TABLE Comments;--"));
            if (ex is SqliteException sqliteEx)
            {
                Assert.DoesNotContain("syntax", sqliteEx.Message, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
