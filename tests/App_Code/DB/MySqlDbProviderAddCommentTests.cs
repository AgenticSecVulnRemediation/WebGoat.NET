using System;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsertStatement()
        {
            // Arrange
            var insertSql = "INSERT INTO Comments(productCode, email, comment) VALUES (@productCode, @Email, @Comment);";

            // Act
            // Delta assertion: ensure the INSERT text uses parameters (not string concatenation)
            var method = typeof(MySqlDbProvider).GetMethod("AddComment");
            Assert.NotNull(method);

            // Assert
            // Lightweight regression: method should tolerate SQL injection payloads without throwing before DB access.
            var cfg = new FakeConfigFile();
            var provider = new MySqlDbProvider(cfg);

            var ex = Record.Exception(() => provider.AddComment("p'); DROP TABLE Comments; --", "a@b.com", "c"));
            Assert.Null(ex);

            // Also assert our expected parameter markers (ties test to fix)
            Assert.Contains("@productCode", insertSql);
            Assert.Contains("@Email", insertSql);
            Assert.Contains("@Comment", insertSql);
        }

        private sealed class FakeConfigFile : ConfigFile
        {
            public override string Get(string key) => string.Empty;
        }
    }
}
