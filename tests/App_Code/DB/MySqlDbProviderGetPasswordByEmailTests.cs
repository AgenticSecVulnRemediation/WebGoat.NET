using System;
using Xunit;

// Assumption: MySqlDbProvider is in namespace OWASP.WebGoat.NET.App_Code.DB.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetPasswordByEmailTests
    {
        [Fact]
        public void GetPasswordByEmail_EmailWithQuote_DoesNotThrowDueToSqlConcat()
        {
            // Arrange
            var provider = CreateProvider();

            // Act
            var ex = Record.Exception(() => provider.GetPasswordByEmail("a'@example.com"));

            // Assert
            // Regression: previously the query concatenated email into SQL; an apostrophe could break it.
            Assert.Null(ex);
        }

        private static MySqlDbProvider CreateProvider()
        {
            var config = new OWASP.WebGoat.NET.App_Code.ConfigFile();
            return new MySqlDbProvider(config);
        }
    }
}
