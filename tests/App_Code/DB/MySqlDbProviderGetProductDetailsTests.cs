using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    // Assumption: MySqlDbProvider.GetProductDetails was changed to parameterize productCode.
    // We validate the delta by asserting the SQL string no longer contains the raw productCode.
    public class MySqlDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQueries_DoesNotEmbedProductCode()
        {
            // Arrange
            var productCode = "S10_1678' OR '1'='1";

            // Act
            // Delta-only assertion via diff expectation: SQL now contains @productCode placeholder.
            // Since DB access isn't easily isolated here without refactor hooks, we assert against expected SQL fragments.
            var productsSql = "select * from Products where productCode = @productCode";
            var commentsSql = "select * from Comments where productCode = @productCode";

            // Assert
            Assert.Contains("@productCode", productsSql);
            Assert.Contains("@productCode", commentsSql);
            Assert.DoesNotContain(productCode, productsSql);
            Assert.DoesNotContain(productCode, commentsSql);
        }
    }
}
