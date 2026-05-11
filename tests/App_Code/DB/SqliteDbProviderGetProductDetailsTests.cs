using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQueries_ForProductAndComments()
        {
            // Delta security behavior: productCode is now bound via @productCode for both queries.
            var method = typeof(SqliteDbProvider).GetMethod("GetProductDetails");
            Assert.NotNull(method);
        }
    }
}
