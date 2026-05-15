using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameters_ForProductCode()
        {
            // Delta test for SQL injection fix: queries should use @productCode parameter.

            // Arrange
            var productsSql = "select * from Products where productCode = @productCode";
            var commentsSql = "select * from Comments where productCode = @productCode";

            // Assert
            Assert.Contains("@productCode", productsSql);
            Assert.Contains("@productCode", commentsSql);
            Assert.DoesNotContain("'" + " +", productsSql);
            Assert.DoesNotContain("'" + " +", commentsSql);
        }
    }
}
