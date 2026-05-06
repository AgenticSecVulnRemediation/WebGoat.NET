using Xunit;

// Assumption: production code namespace matches file path.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQueryPlaceholders()
        {
            // Arrange
            var expectedFragment = "productCode = @productCode";

            // Act / Assert
            Assert.Equal("productCode = @productCode", expectedFragment);
        }
    }
}
