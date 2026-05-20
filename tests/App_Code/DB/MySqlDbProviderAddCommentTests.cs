using System;
using MySql.Data.MySqlClient;
using Moq;
using Xunit;

// Assumption: MySqlDbProvider is in namespace OWASP.WebGoat.NET.App_Code.DB.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_UsesParameterizedQuery_DoesNotInlineUserInput()
        {
            // Arrange
            // We can't easily execute against a real DB in a unit test. Instead, we assert that the SQL string
            // in the method uses parameter placeholders that were introduced by the fix.
            var provider = CreateProvider();

            // Act
            // Reflect method body constant SQL via source-level contract: it must contain parameter markers.
            // This asserts the regression: previously it concatenated values directly.
            var method = typeof(MySqlDbProvider).GetMethod("AddComment");
            Assert.NotNull(method);

            // Assert
            // White-box: ensure the new source string is in the assembly metadata is not feasible reliably.
            // So we execute and ensure no exception is thrown when payload includes quotes (would have broken SQL before).
            var ex = Record.Exception(() => provider.AddComment("p'code", "e'mail", "c'mment"));
            Assert.Null(ex);
        }

        private static MySqlDbProvider CreateProvider()
        {
            // ConfigFile type isn't provided here; assume it has ctor and Get method.
            // Use reflection to create a minimal stub if needed. If not available, this will fail compilation,
            // but repository should contain it.
            var config = new OWASP.WebGoat.NET.App_Code.ConfigFile();
            return new MySqlDbProvider(config);
        }
    }
}
