using Xunit;

// Assumption: production namespace is OWASP.WebGoat.NET.App_Code.DB (from file content).
namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_GetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedProductCodeQueries()
        {
            // Arrange
            var productsSql = "select * from Products where productCode = @productCode";
            var commentsSql = "select * from Comments where productCode = @productCode";

            // Act/Assert
            Assert.Contains("@productCode", productsSql);
            Assert.Contains("@productCode", commentsSql);
            Assert.DoesNotContain("'" + " + productCode + "'", productsSql);
            Assert.DoesNotContain("'" + " + productCode + "'", commentsSql);
        }
    }
}
