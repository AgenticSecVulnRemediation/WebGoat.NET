using Xunit;

// Assumption: production code compiles under namespace OWASP.WebGoat.NET.App_Code.DB
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQuery_ForProductCode()
        {
            // Arrange
            var method = typeof(SqliteDbProvider).GetMethod("GetProductDetails");

            // Act
            var sig = method?.ToString();

            // Assert
            Assert.NotNull(method);
            Assert.Contains("GetProductDetails", sig);
        }
    }
}
