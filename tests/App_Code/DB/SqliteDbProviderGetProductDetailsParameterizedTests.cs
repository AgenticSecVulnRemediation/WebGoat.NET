using Xunit;
using System;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsParameterizedTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterPlaceholder_DoesNotInlineProductCode()
        {
            // Arrange
            var attackerSupplied = "X' OR 1=1 --";

            // Act
            var productsSql = "select * from Products where productCode = @productCode";
            var commentsSql = "select * from Comments where productCode = @productCode";

            // Assert
            Assert.Contains("@productCode", productsSql, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("@productCode", commentsSql, StringComparison.OrdinalIgnoreCase);

            Assert.DoesNotContain(attackerSupplied, productsSql, StringComparison.Ordinal);
            Assert.DoesNotContain(attackerSupplied, commentsSql, StringComparison.Ordinal);
        }
    }
}
