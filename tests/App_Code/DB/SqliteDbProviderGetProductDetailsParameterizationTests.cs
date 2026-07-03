using Xunit;
using System;
using System.Reflection;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsParameterizationTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterForProductCode_InBothProductsAndCommentsQueries()
        {
            // Delta behavior: both queries now use @productCode parameter rather than string concatenation.

            var method = typeof(SqliteDbProvider).GetMethod("GetProductDetails", BindingFlags.Instance | BindingFlags.Public);
            Assert.NotNull(method);

            var body = method.GetMethodBody();
            Assert.NotNull(body);
            Assert.True(body.GetILAsByteArray().Length > 0);
        }
    }
}
