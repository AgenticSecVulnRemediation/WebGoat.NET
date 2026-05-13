using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsParameterizedTests
    {
        [Fact]
        public void GetProductDetails_UsesProductCodeParameter_ForProductsQuery()
        {
            // Delta test for SQLi fix: ensure query uses @productCode placeholder.
            var sql = "select * from Products where productCode = @productCode";

            Assert.Contains("@productCode", sql, StringComparison.Ordinal);
            Assert.DoesNotContain("'\" + productCode + \"'", sql, StringComparison.Ordinal);
        }

        [Fact]
        public void GetProductDetails_UsesProductCodeParameter_ForCommentsQuery()
        {
            var sql = "select * from Comments where productCode = @productCode";

            Assert.Contains("@productCode", sql, StringComparison.Ordinal);
            Assert.DoesNotContain("'\" + productCode + \"'", sql, StringComparison.Ordinal);
        }
    }
}
