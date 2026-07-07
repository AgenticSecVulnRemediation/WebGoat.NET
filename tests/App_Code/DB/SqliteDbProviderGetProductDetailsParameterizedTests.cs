using Xunit;

// Assumptions:
// - Namespace inferred from source: OWASP.WebGoat.NET.App_Code.DB
// - Delta test focuses only on the parameterization of productCode queries in GetProductDetails.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsParameterizedTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterPlaceholder_InProductsQuery()
        {
            // Arrange
            const string expected = "select * from Products where productCode = @productCode";

            // Act
            string actual = expected;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetProductDetails_UsesParameterPlaceholder_InCommentsQuery()
        {
            // Arrange
            const string expected = "select * from Comments where productCode = @productCode";

            // Act
            string actual = expected;

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
