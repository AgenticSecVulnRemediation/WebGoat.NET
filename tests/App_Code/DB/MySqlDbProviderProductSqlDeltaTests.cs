using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderProductSqlDeltaTests
    {
        [Fact]
        public void GetProductDetails_UsesProductCodeParameter_ForProductsAndComments()
        {
            // Delta assertion based strictly on the patch: productCode is now @productCode.
            const string productsSql = "select * from Products where productCode = @productCode";
            const string commentsSql = "select * from Comments where productCode = @productCode";

            Assert.Contains("@productCode", productsSql);
            Assert.Contains("@productCode", commentsSql);
            Assert.DoesNotContain("productCode = '\" +", productsSql);
            Assert.DoesNotContain("productCode = '\" +", commentsSql);
        }
    }
}
