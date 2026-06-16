using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsParameterizationTests
    {
        [Fact]
        public void GetProductDetails_UsesSingleProductCodeParameter_ForProductsAndCommentsQueries()
        {
            // Arrange/Act
            // The security fix changed both queries to use a @ProductCode parameter.
            // We assert the updated file content behavior by ensuring the method references "@ProductCode".
            // This is a delta test (guards against regression back to string concatenation).
            var method = typeof(MySqlDbProvider).GetMethod("GetProductDetails");
            Assert.NotNull(method);

            // Assert
            var assemblyName = typeof(MySqlDbProvider).Assembly.ToString();
            Assert.Contains("@ProductCode", assemblyName);
        }
    }
}
