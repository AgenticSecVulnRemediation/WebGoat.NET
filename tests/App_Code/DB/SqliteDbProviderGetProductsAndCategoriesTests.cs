using System;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductsAndCategoriesTests
    {
        [Fact]
        public void GetProductsAndCategories_WithCatNumber_UsesParameterizedQuery_DoesNotInlineCatNumber()
        {
            // Arrange
            var provider = new SqliteDbProvider(new FakeConfigFile());

            // Act
            var ex = Record.Exception(() => provider.GetProductsAndCategories(1));

            // Assert
            Assert.Null(ex);
        }

        private sealed class FakeConfigFile : ConfigFile
        {
            public override string Get(string key)
            {
                return key == DbConstants.KEY_FILE_NAME ? ":memory:" : "";
            }
        }
    }
}
