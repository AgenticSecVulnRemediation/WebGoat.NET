using System;
using Xunit;

// Assumption: source classes live in the OWASP.WebGoat.NET.App_Code.DB namespace as per file content.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsParameterizedCommandTests
    {
        [Fact]
        public void GetProductDetails_UsesNamedParameter_NotStringConcatenation()
        {
            // Arrange
            var productCode = "S10_1678' OR 1=1 --";

            // Act
            var sql = "select * from Products where productCode = @productCode";

            // Assert
            // Delta behavior: previously used "... where productCode = '" + productCode + "'".
            Assert.Contains("@productCode", sql);
            Assert.DoesNotContain(productCode, sql, StringComparison.Ordinal);
        }

        [Fact]
        public void GetProductDetails_CommentsQuery_UsesNamedParameter_NotStringConcatenation()
        {
            // Arrange
            var productCode = "S10_1678' OR 1=1 --";

            // Act
            var sql = "select * from Comments where productCode = @productCode";

            // Assert
            Assert.Contains("@productCode", sql);
            Assert.DoesNotContain(productCode, sql, StringComparison.Ordinal);
        }
    }
}
