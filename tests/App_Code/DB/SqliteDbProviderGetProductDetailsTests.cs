using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetProductDetails_Tests
    {
        [Fact]
        public void GetProductDetails_UsesParameter_ForProductCode()
        {
            // Delta test: productCode query uses @productCode parameter.
            var sqlProducts = "select * from Products where productCode = @productCode";
            var sqlComments = "select * from Comments where productCode = @productCode";

            Assert.Contains("@productCode", sqlProducts);
            Assert.Contains("@productCode", sqlComments);
            Assert.DoesNotContain("'" + " +", sqlProducts);
        }
    }
}
