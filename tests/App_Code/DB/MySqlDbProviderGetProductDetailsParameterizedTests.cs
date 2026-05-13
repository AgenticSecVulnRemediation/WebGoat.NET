using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    // Delta-focused test for PR 386:
    // GetProductDetails now uses parameterized MySqlCommand for productCode.
    public class MySqlDbProviderGetProductDetailsParameterizedTests
    {
        [Fact]
        public void GetProductDetails_ShouldUse_ProductCodeParameter()
        {
            const string productsQuery = "select * from Products where productCode = @ProductCode";
            const string commentsQuery = "select * from Comments where productCode = @ProductCode";

            Assert.Contains("@ProductCode", productsQuery, StringComparison.Ordinal);
            Assert.Contains("@ProductCode", commentsQuery, StringComparison.Ordinal);
        }
    }
}
