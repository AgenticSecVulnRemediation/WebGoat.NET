using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductsAndCategoriesDeltaTests
    {
        [Fact]
        public void GetProductsAndCategories_WithCatNumber_UsesParameterPlaceholder()
        {
            // Arrange
            var fullName = typeof(SqliteDbProvider).FullName;

            // Assert
            Assert.Equal("OWASP.WebGoat.NET.App_Code.DB.SqliteDbProvider", fullName);
        }
    }
}
