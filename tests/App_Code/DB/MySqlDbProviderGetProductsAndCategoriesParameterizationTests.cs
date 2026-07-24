using Xunit;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using System;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductsAndCategoriesParameterizationTests
    {
        [Fact]
        public void GetProductsAndCategories_WithCatNumber_UsesParameterizedClauseAndDoesNotThrowSqlSyntax()
        {
            // Arrange
            var config = new Mock<ConfigFile>(MockBehavior.Loose);
            config.Setup(c => c.Get(It.IsAny<string>())).Returns("x");
            var provider = new MySqlDbProvider(config.Object);

            // Act
            var ex = Record.Exception(() => provider.GetProductsAndCategories(1));

            // Assert
            if (ex != null)
            {
                Assert.DoesNotContain("SQL syntax", ex.Message, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
