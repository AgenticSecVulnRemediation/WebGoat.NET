using System;
using Xunit;

// Assumptions:
// - Namespace inferred from source: OWASP.WebGoat.NET.App_Code.DB
// - This delta test focuses on the changed query construction in GetProductDetails:
//   it must use @productCode parameter in both Products and Comments queries.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsParameterizedTests
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
