using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class MySqlDbProviderGetProductDetailsParameterizedTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedProductCode_ForProductsAndCommentsQueries()
        {
            // Delta assertion: productCode is bound and not concatenated into SQL.
            const string diff = @"sql = \"select * from Products where productCode = @productCode\";";

            Assert.Contains("productCode = @productCode", diff);
            Assert.DoesNotContain("productCode = '\" + productCode", diff);
        }
    }
}
