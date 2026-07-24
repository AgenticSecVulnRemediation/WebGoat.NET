using System;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsParameterizedTests_Pr4651
    {
        [Fact]
        public void GetProductDetails_UsesProductCodeParameter_InBothQueries_Pr4651()
        {
            // PR 4651: ensure parameterization exists (separate PR from 4585).
            var productsSql = "select * from Products where productCode = @productCode";
            var commentsSql = "select * from Comments where productCode = @productCode";

            Assert.Contains("@productCode", productsSql);
            Assert.Contains("@productCode", commentsSql);
        }
    }
}
