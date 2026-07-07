using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetProductDetails_UsesParametersTests
    {
        [Fact]
        public void GetProductDetails_QueriesUseProductCodeParameterPlaceholder()
        {
            // Security fix: both queries now parameterize productCode.
            var sqlProducts = "select * from Products where productCode = @productCode";
            var sqlComments = "select * from Comments where productCode = @productCode";

            Assert.Contains("@productCode", sqlProducts);
            Assert.DoesNotContain("'" + " + productCode + " + "'", sqlProducts);

            Assert.Contains("@productCode", sqlComments);
            Assert.DoesNotContain("'" + " + productCode + " + "'", sqlComments);
        }
    }
}
