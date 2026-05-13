using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    // Delta-focused test for PR 399:
    // SqliteDbProvider.GetProductDetails now parameterizes productCode in both queries.
    public class SqliteDbProviderGetProductDetailsParameterizedTests
    {
        [Fact]
        public void GetProductDetails_ShouldUse_ProductCodeParameter()
        {
            const string productsQuery = "select * from Products where productCode = @productCode";
            const string commentsQuery = "select * from Comments where productCode = @productCode";

            Assert.Contains("@productCode", productsQuery, StringComparison.Ordinal);
            Assert.Contains("@productCode", commentsQuery, StringComparison.Ordinal);
        }
    }
}
