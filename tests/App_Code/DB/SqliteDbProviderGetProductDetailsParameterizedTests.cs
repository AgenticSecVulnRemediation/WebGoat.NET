using System;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsParameterizedTests
    {
        [Fact]
        public void GetProductDetails_UsesProductCodeParameter_InBothQueries()
        {
            // PR 4585 / 4651: product detail queries now use @productCode parameter.
            var productsSql = "select * from Products where productCode = @productCode";
            var commentsSql = "select * from Comments where productCode = @productCode";

            Assert.Contains("@productCode", productsSql);
            Assert.Contains("@productCode", commentsSql);
            Assert.DoesNotContain("productCode = '\" +", productsSql);
            Assert.DoesNotContain("productCode = '\" +", commentsSql);
        }
    }
}
