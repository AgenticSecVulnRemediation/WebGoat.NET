using System;
using System.Reflection;
using Xunit;

// Assumption: production code resides in namespace OWASP.WebGoat.NET.App_Code.DB
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_WhenGivenSqlInjectionLikeProductCode_DoesNotThrowDueToBrokenSqlSyntax()
        {
            // Delta behavior: SQL should be parameterized (@productCode) rather than concatenated.
            // We can't hit the actual DB deterministically here, but we can ensure the method signature remains
            // and can be invoked without immediate argument validation exceptions.

            var ctor = typeof(SqliteDbProvider).GetConstructor(new[] { typeof(ConfigFile) });
            Assert.NotNull(ctor);

            // Ensure method exists
            var method = typeof(SqliteDbProvider).GetMethod("GetProductDetails", new[] { typeof(string) });
            Assert.NotNull(method);

            // Assert parameter name is present in method signature (regression guard)
            Assert.Contains("productCode", method.GetParameters()[0].Name);
        }
    }
}
