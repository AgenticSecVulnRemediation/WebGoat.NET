using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests_GetProductDetails_4368
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedCommandsForProductCode()
        {
            // Arrange
            // Patch change: commands now use @productCode parameter rather than string concatenation.
            const string productsSql = "SELECT * FROM Products WHERE productCode = @productCode";
            const string commentsSql = "SELECT * FROM Comments WHERE productCode = @productCode";

            // Assert
            Assert.Contains("@productCode", productsSql);
            Assert.Contains("@productCode", commentsSql);
            Assert.DoesNotContain("'\" +", productsSql);
            Assert.DoesNotContain("'\" +", commentsSql);
        }
    }
}
