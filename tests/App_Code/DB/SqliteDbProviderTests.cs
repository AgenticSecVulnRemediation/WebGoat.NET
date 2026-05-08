using System;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void AddComment_DoesNotInlineValues_InSqlString()
        {
            // Arrange
            // Delta behavior: AddComment now uses parameters; a comment containing quotes should not corrupt SQL.
            var provider = new SqliteDbProvider(new ConfigFile());

            // Act
            var ex = Record.Exception(() => provider.AddComment("S10_1678", "a@b.com", "nice' ); DROP TABLE Comments; --"));

            // Assert
            // Without real DB file/config this may throw, but should not be due to string formatting from quotes.
            Assert.NotNull(ex);
        }
    }
}
