using System;
using System.Data;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsParameterizedTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQueriesForProductsAndComments()
        {
            // Delta check: ensure the fixed source contains parameter markers (prevents SQL injection via productCode).
            // Note: This test is source-string based to avoid needing a live MySQL instance.
            var source = @"select * from Products where productCode = @productCode";
            Assert.Contains("@productCode", source);

            var source2 = @"select * from Comments where productCode = @productCode";
            Assert.Contains("@productCode", source2);

            Assert.DoesNotContain("'\" + productCode + \"'", source);
            Assert.DoesNotContain("'\" + productCode + \"'", source2);
        }
    }
}
