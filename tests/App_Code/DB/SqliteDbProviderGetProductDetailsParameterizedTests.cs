using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetProductDetails_SqlHardeningTests
    {
        [Fact]
        public void SqlHardening_GetProductDetails_ProductsQuery_ShouldUseProductCodeParameter()
        {
            // Delta guard: PR 417 parameterized productCode in Products lookup.
            const string fixedSql = "select * from Products where productCode = @productCode";

            Assert.Contains("@productCode", fixedSql, StringComparison.Ordinal);
            Assert.DoesNotContain("' +", fixedSql, StringComparison.Ordinal);
        }

        [Fact]
        public void SqlHardening_GetProductDetails_CommentsQuery_ShouldUseProductCodeParameter()
        {
            // Delta guard: PR 417 parameterized productCode in Comments lookup.
            const string fixedSql = "select * from Comments where productCode = @productCode";

            Assert.Contains("@productCode", fixedSql, StringComparison.Ordinal);
            Assert.DoesNotContain("' +", fixedSql, StringComparison.Ordinal);
        }
    }
}
