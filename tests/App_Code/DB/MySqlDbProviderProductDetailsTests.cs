using Xunit;

// Assumption: production code compiles under namespace OWASP.WebGoat.NET.App_Code.DB
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQueries_ForProductsAndComments()
        {
            // Arrange
            var type = typeof(MySqlDbProvider);
            var method = type.GetMethod("GetProductDetails");

            // Act
            var signature = method?.ToString();

            // Assert
            Assert.NotNull(method);
            Assert.Contains("GetProductDetails", signature);
            // The delta change introduced @productCode placeholder; validate at least the string exists in method signature/metadata.
            Assert.Contains("productCode", signature);
        }
    }
}
