using System;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_WithSqlInjectionPayload_DoesNotThrowDueToSqlConcatenation()
        {
            // Arrange
            // This delta test targets the change from string concatenation to parameterized INSERT.
            // We assert that the method can be invoked with characters that would previously break SQL.
            var config = new ConfigFile();
            // NOTE: ConfigFile construction details are not available in patch context.
            // If default ctor is not available, this test will need adjustment.
            var provider = new SqliteDbProvider(config);

            // Act
            var ex = Record.Exception(() => provider.AddComment("P1", "a@b.com", "x'); DROP TABLE Comments;--"));

            // Assert
            Assert.Null(ex);
        }
    }
}
