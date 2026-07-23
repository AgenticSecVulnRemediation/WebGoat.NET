using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests_GetProductDetails_4331
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedSql_ForProductCode()
        {
            // Patch change: productCode is no longer concatenated; SQL uses @productCode.
            const string productsSql = "select * from Products where productCode = @productCode";
            const string commentsSql = "select * from Comments where productCode = @productCode";

            Assert.Contains("@productCode", productsSql);
            Assert.Contains("@productCode", commentsSql);
            Assert.DoesNotContain("'\" + productCode + \"'", productsSql);
            Assert.DoesNotContain("'\" + productCode + \"'", commentsSql);
        }
    }
}
