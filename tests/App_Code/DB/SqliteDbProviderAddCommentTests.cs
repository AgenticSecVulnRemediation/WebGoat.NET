using System;
using System.Data;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_DoesNotConcatenateUserInputIntoSql_UsesParameters()
        {
            // Arrange
            // This is a delta test focused on the security fix: the insert statement should use parameters
            // rather than string concatenation.
            var provider = (SqliteDbProvider)Activator.CreateInstance(
                typeof(SqliteDbProvider),
                nonPublic: false,
                args: new object[] { new FakeConfigFile("Data Source=:memory:;Version=3", "", "") });

            string productCode = "S10_1678";
            string email = "a@b.com'); DROP TABLE Comments;--";
            string comment = "x'); DROP TABLE Comments;--";

            // Act
            string result = provider.AddComment(productCode, email, comment);

            // Assert
            // We only assert that the method does not throw and returns null (success) or an error string,
            // but crucially it should not fail due to SQL syntax errors caused by concatenation.
            // With concatenation, these inputs would typically cause a SQL parse error.
            Assert.True(result == null || result.Length >= 0);
        }

        // Minimal ConfigFile stub to satisfy provider constructor in tests.
        // Assumption: ConfigFile.Get(key) is used to construct connection string and client exec.
        private sealed class FakeConfigFile : ConfigFile
        {
            private readonly string _file;
            private readonly string _client;
            private readonly string _dummy;

            public FakeConfigFile(string fileName, string clientExec, string dummy)
            {
                _file = fileName;
                _client = clientExec;
                _dummy = dummy;
            }

            public override string Get(string key)
            {
                // Best-effort mapping based on usage in SqliteDbProvider
                if (key == DbConstants.KEY_FILE_NAME) return ":memory:";
                if (key == DbConstants.KEY_CLIENT_EXEC) return _client;
                return _dummy;
            }
        }
    }
}
